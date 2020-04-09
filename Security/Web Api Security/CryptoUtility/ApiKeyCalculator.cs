using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace BAFactory.Fx.Samples.WebApiSecurity.CryptoUtility
{
    public class ApiKeyCalculator
    {
        public string CalculateHmacHash(string url, string method, NameValueCollection request, string appId, string apiKey)
        {
            string requestTimeStamp = EpochTime.GetNow();

            string nonce = Guid.NewGuid().ToString("N");

            var content = ConvertToContentString(request);

            return CalculateHmacHash(url, method, content, appId, apiKey, nonce, requestTimeStamp);
        }

        public string CalculateHmacHash(string url, string method, string request, string appId, string apiKey, string nonce, string requestTimeStamp)
        {
            var result = string.Empty;

            string requestContentBase64String = string.Empty;
            string requestUri = HttpUtility.UrlEncode(url);

            byte[] content = Encoding.UTF8.GetBytes(request);
            MD5 md5 = MD5.Create();
            byte[] requestContentHash = md5.ComputeHash(content);
            requestContentBase64String = Convert.ToBase64String(requestContentHash);

            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", appId, method, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyByteArray = Convert.FromBase64String(apiKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                result = string.Format("{0}:{1}:{2}:{3}", appId, requestSignatureBase64String, nonce, requestTimeStamp);
            }

            return result;
        }

        private string ConvertToContentString(NameValueCollection request)
        {
            var result = new StringBuilder();

            if (request != null)
            {
                foreach (string key in request.Keys)
                {
                    result.Append(string.Concat(key, "=", request[key].ToString(), "&"));
                }
            }

            return result.Remove(result.Length - 1, 1).ToString();
        }

    }
}

