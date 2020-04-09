using System.Collections.Generic;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.Areas.Membership.Extensions;
using BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting;
using BAFactory.Fx.Security.MVCExtension;
using System.Reflection;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    public abstract class BaseController : MembershipSessionInfoController
    {
        protected MembershipManager manager;

        protected FilterInformation FilterInfo
        {
            get
            {
                return Session[MembershipSessionDataKeys.Filter] as FilterInformation;
            }
            set
            {
                if (Session != null) Session[MembershipSessionDataKeys.Filter] = value;
            }
        }

        protected BaseController():base()
        {
            InitializeFilterInformation();

            InitializeManager();

            LoadControllerData();
        }

        private void InitializeFilterInformation()
        {
            FilterInfo = new FilterInformation();
        }

        // TODO: crear los controles reutilizables y/o almacenar los filtros comunes en la aplicación
        // es abstract porque quiero que cada controller cargue sólo los datos que necesita
        protected abstract void LoadViewData();

        protected abstract void LoadControllerData();

        protected abstract void SetViewBagData();

        protected void UpdateFilterInformation(int? pageNumber)
        {
            FilterInfo.PageNumber = pageNumber ?? 1;

            if (HttpContext == null || HttpContext.Request == null
                || HttpContext.Request.Form == null || HttpContext.Request.Form.Count < 1)
            {
                return;
            }
            
            FilterInfo = UpdateFilterInformation();
        }

        protected void ApplySorting<T>(ref List<T> elements)
        {
            ApplySorting<T, DefaultSortingDevice<T>>(ref elements, null);
        }

        protected void ApplySorting<T>(ref List<T> elements, string member)
        {
            ApplySorting<T, DefaultSortingDevice<T>>(ref elements, member);
        }

        protected void ApplySorting<Tsorted, Tsorter>(ref List<Tsorted> elements)
        {
            ApplySorting<Tsorted, Tsorter>(ref elements, null);
        }

        protected void ApplySorting<Tsorted, Tsorter>(ref List<Tsorted> elements, string member)
        {
            ISortingDevice<Tsorted> sortDevice = SortingFactory.GetSortingDevice<Tsorted, Tsorter>();

            if (sortDevice != null)
            {
                if (member == null)
                {
                    sortDevice.ApplySorting(ref elements);
                }
                else
                {
                    sortDevice.ApplySorting(ref elements, member);
                }
            }
        }

        protected JsonResult ReturnAllowedJsonGet(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private void InitializeManager()
        {
            manager = new MembershipManager();
        }

        private FilterInformation UpdateFilterInformation()
        {
            FilterInformation filterInfo = new FilterInformation();
            filterInfo.PageNumber = 1;
          
            foreach (string formFieldName in HttpContext.Request.Form.AllKeys)
            {
                if (!formFieldName.StartsWith("FILTER_")) continue;

                object propertyInstanceReference = null;

                PropertyInfo property = GetTargetProperty(ref filterInfo, formFieldName, out propertyInstanceReference);

                object boxedValue = GetObjectValue(property, HttpContext.Request.Form[formFieldName]);

                if (propertyInstanceReference == null || property == null || boxedValue == null) continue;

                property.SetValue(propertyInstanceReference, boxedValue, null);
            }

            return filterInfo;
        }

        private object GetObjectValue(PropertyInfo property, string p)
        {
            object result = null;
            switch (property.PropertyType.ToString())
            {
                case "System.Int64":
                    result = long.Parse(p);
                    break;
                case "System.Int32":
                    result = int.Parse(p);
                    break;
                default:
                    result = p;
                    break;
            }
            return result;
        }

        private PropertyInfo GetTargetProperty(ref FilterInformation filterInfo, string formFieldName, out object propertyInstanceReference)
        {
            string[] formFieldNameElements = formFieldName.Split(new char[] { '_' });

            propertyInstanceReference = filterInfo;

            PropertyInfo propertyInfo = GetTargetProperty(formFieldNameElements, ref propertyInstanceReference);

            return propertyInfo;
        }

        private PropertyInfo GetTargetProperty(string[] formFieldNameElements, ref object objectReference)
        {
            PropertyInfo propertyRef = null;
            object parentReference = null;

            for (int i = 1; i < formFieldNameElements.Length; ++i)
            {
                parentReference = objectReference;
                propertyRef = GetTargetProperty(formFieldNameElements[i], ref objectReference);
            }
            objectReference = parentReference;

            return propertyRef;
        }

        private PropertyInfo GetTargetProperty(string propertyName, ref object objectReference)
        {
            PropertyInfo property = objectReference.GetType().GetProperty(propertyName);
            objectReference = property.GetValue(objectReference, null);
            return property;
        }
    }
}