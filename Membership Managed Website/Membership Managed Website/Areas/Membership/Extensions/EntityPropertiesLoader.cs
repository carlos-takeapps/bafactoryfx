using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions
{
    public static class EntityPropertiesLoader
    {
        public static T UpdateProperties<T>(ref T instance, NameValueCollection properties)
        {
            SetPropertiesValues(ref instance, properties);

            return instance;
        }
        public static T PopulateProperties<T>(NameValueCollection properties)
        {
            T instance = GetInstance<T>();

            return UpdateProperties(ref instance, properties);
        }

        private static T GetInstance<T>()
        {
            Type t = typeof(T);
            T instance = (T)Activator.CreateInstance(t);

            if (instance == null)
            {
                throw new NullReferenceException();
            }

            return instance;
        }

        private static void SetPropertiesValues<T>(ref T instance, NameValueCollection valuesCollection)
        {
            foreach (string name in valuesCollection.Keys)
            {
                string value = valuesCollection[name];
                SetPropertyValue<T>(ref instance, name, value);
            }
        }

        private static void SetPropertyValue<T>(ref T instance, string name, string value)
        {
            PropertyInfo property = GetTypeProperty<T>(name);

            if (property == null)
                return; 

            object boxedValue = GetTypedValue(property.PropertyType, value);

            property.SetValue(instance, boxedValue, null);
        }

        private static PropertyInfo GetTypeProperty<T>(string name)
        {
            Type t = typeof(T);
            return t.GetProperty(name);
        }

        private static object GetTypedValue(Type type, string p)
        {
            object result = null;
            switch (type.FullName)
            {
                case "System.String":
                    result = p;
                    break;
                case "System.Int16":
                case "System.Int32":
                    int parsedInt = int.MinValue;
                    int.TryParse(p, out parsedInt);
                    result = parsedInt;
                    break;
                case "System.Int64":
                    long parsedLong = long.MinValue;
                    long.TryParse(p, out parsedLong);
                    result = parsedLong;
                    break;
                default:
                    throw new InvalidCastException("Tipo de dato no definido.");
            }
            return result;
        }
    }
}