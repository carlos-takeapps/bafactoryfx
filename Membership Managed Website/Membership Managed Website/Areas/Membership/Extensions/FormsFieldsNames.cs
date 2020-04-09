using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions
{
    public static class FormsFieldsNames
    {
        /// <summary>
        /// Las propiedades de esta clase deben contener valores acordes a la la estructura 
        /// de nombres de la propiedades de la clase FilterInformation antecedidas por el texto "FILTER_"
        /// </summary>
        public static class FilterForm
        {
            public static string UserId
            {
                get { return "FILTER_User_Id"; }
                private set { return; }
            }
            
            public static string UserName
            {
                get { return "FILTER_User_Name"; }
                private set { return; }
            }

            public static string AreaId
            {
                get { return "FILTER_Area_Id"; }
                private set { return; }
            }

            public static string Sort
            {
                get { return "FILTER_Sort"; }
                private set { return; }
            }
        }

        public static string Username
        {
            get { return "USERNAME"; }
            private set { return; }
        }

        public static string ActionId
        {
            get { return "IdAction"; }
            private set { return; }
        }

        public static string ModuleId
        {
            get { return "IdModule"; }
            private set { return; }
        }

        public static string AreaId
        {
            get { return "IdArea"; }
            private set { return; }
        }

        public static string UserId
        {
            get { return "IdUser"; }
            private set { return; }
        }
    }
}