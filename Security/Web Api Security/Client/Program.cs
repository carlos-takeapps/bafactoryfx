using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAFactory.Fx.Samples.WebApiSecurity.Client
{
    class Program
    {
        static string _email = "user@email.com";
        static string _password = "thePassw0rd.";

        static string APPId = "4d53bce03ec34c0a911182d4c228ee6c";
        static string APIKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";

        static void Main(string[] args)
        {
            // CreateLogin();
            var client = new SecureApiClient(APPId, APIKey);

            client.Authenticate("http://apisec.pocs.localhost/Token", _email, _password);

            GetSecureContent(client);

            PostSecureContent(client);

            Console.ReadLine();
        }

        static void GetSecureContent(SecureApiClient client)
        {
            var url = string.Concat("http://apisec.pocs.localhost/api/test/", HttpUtility.UrlEncode("My_Friend"));

            var response = client.DownloadSecureData(url);

            Console.WriteLine(Encoding.UTF8.GetString(response));
        }

        static void PostSecureContent(SecureApiClient client)
        {
            var url = "http://apisec.pocs.localhost/api/test";

            var response = client.UploadSecureData(url, new { FirstName = "Carlos", LastName = "Salvatore" });

            Console.WriteLine(Encoding.UTF8.GetString(response));
        }

        static void CreateLogin()
        {
            var client = new SecureApiClient(APPId, APIKey);

            client.CreateLogin("http://apisec.pocs.localhost/api/Account/Register", _email, _password);
        }
    }
}
