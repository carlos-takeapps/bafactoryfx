using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;
using System.Reflection;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting
{

    // Ordena los elementos de manera automática según las propiedades Nombre o Id
    internal class DefaultSortingDevice<T> : ISortingDevice<T>
    {
        private List<string> defaultProperties { get; set; }

        private List<T> sortingElements { get; set; }

        public DefaultSortingDevice()
        {
            defaultProperties = new List<string>() { "Name", "Description", "Id" };
        }

        public void ApplySorting(ref List<T> elements)
        {
            sortingElements = elements;

            PropertyInfo idProperty = null;
            PropertyInfo compareProperty = GetCompareProperty<T>(out idProperty);

            SortElements(compareProperty, idProperty);

            if (elements.Count() != sortingElements.Count())
                throw new ApplicationException("Error en el ordenamiento");

            elements = sortingElements;
        }

        public void ApplySorting(ref List<T> elements, string member)
        {
            if (member == null)
                throw new ArgumentNullException("Se debe especificar la propiedad de ordenamiento");

            defaultProperties = new List<string>() { member };

            ApplySorting(ref elements);
        }

        private PropertyInfo GetCompareProperty<U>(out PropertyInfo idProperty)
        {
            PropertyInfo result = null;

            foreach (string propertyName in defaultProperties)
            {
                PropertyInfo pi = typeof(U).GetProperty(propertyName);
                if (pi != null)
                {
                    result = pi;
                    break;
                }
            }

            idProperty = typeof(U).GetProperty("Id");

            return result;
        }

        private void SortElements(PropertyInfo compareProperty, PropertyInfo idProperty)
        {
            List<long> sortedIds = null;

            switch (compareProperty.PropertyType.ToString())
            {
                case "System.String":
                    sortedIds = GetIdsSortedByStringProperty(compareProperty, idProperty);
                    break;
                case "System.Int32":
                case "System.Int64":
                    sortedIds = GetSortedIds(compareProperty);
                    break;
                default:
                    throw new ApplicationException("La propiedad no es de un tipo para la cual exista un ordenamiento implementado.");
            }

            SortElements(sortedIds, idProperty);
        }

        private List<long> GetSortedIds(PropertyInfo pi)
        {
            List<long> ids = new List<long>();
            foreach (var element in sortingElements)
            {
                ids.Add(long.Parse(pi.GetValue(element, null).ToString()));
            }

            ids.Sort();

            return ids;
        }

        private List<long> GetIdsSortedByStringProperty(PropertyInfo compareProperty, PropertyInfo idProperty)
        {
            List<long> result = new List<long>();

            List<Tuple<long, string>> idNamePair = new List<Tuple<long, string>>();

            foreach (var element in sortingElements)
            {
                string name = compareProperty.GetValue(element, null).ToString();
                long id = long.Parse(idProperty.GetValue(element, null).ToString());

                idNamePair.Add(new Tuple<long, string>(id, name));
            }

            idNamePair.Sort(new Comparison<Tuple<long, string>>(SortIdNamePair));

            foreach (var tupla in idNamePair)
            {
                result.Add(tupla.Item1);
            }

            return result;
        }

        private static int SortIdNamePair(Tuple<long, string> a, Tuple<long, string> b)
        {
            return string.Compare(a.Item2, b.Item2);
        }

        private void SortElements(List<string> sortedNames, PropertyInfo pi)
        {
            List<T> sortedElements = new List<T>();
            foreach (string sortedName in sortedNames)
            {
                foreach (T element in sortingElements)
                {
                    string elementName = pi.GetValue(element, null).ToString();
                    if (elementName == sortedName)
                    {
                        sortedElements.Add(element);
                    }
                }
            }

            sortingElements = sortedElements;
        }
        private void SortElements(List<long> sortedIds, PropertyInfo idProperty)
        {
            List<T> sortedElements = new List<T>();

            foreach (long sortedId in sortedIds)
            {
                foreach (T element in sortingElements)
                {
                    long elementId = long.Parse(idProperty.GetValue(element, null).ToString());
                    if (elementId == sortedId)
                    {
                        sortedElements.Add(element);
                        continue;
                    }
                }
            }
            sortingElements = sortedElements;
        }

    }
}