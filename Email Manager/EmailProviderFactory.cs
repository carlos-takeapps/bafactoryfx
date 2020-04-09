using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Utilities.Email
{
    public class EmailProviderFactory
    {
        public static T CreateProvider<T>(string server, int port, string username, string password, bool ssl)
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { server, port, username, password, ssl });
        }
    }
}
