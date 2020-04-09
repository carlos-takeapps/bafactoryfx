using BAFactory.Fx.Samples.WebApiSecurity.CryptoUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BAFactory.Fx.Samples.WebApiSecurity.Client
{
    public class SecureApiClient : WebClient
    {
        private AuthenticationResponse AuthenticationInfo { get; set; }

        private string APPId { get; set; }
        private string APIKey { get; set; }


        public SecureApiClient(string appId, string appKey) : base()
        {
            APPId = appId;
            APIKey = appKey;
        }

        public void Authenticate(string url, string username, string password)
        {
            var reqparm = new NameValueCollection() {
                { "grant_type", "password" },
                { "username", username },
                { "password", password } };

            var response = base.UploadValues(url, "POST", reqparm);

            var authentication = ExtractAccessToken(response);

            AuthenticationInfo = authentication;

            base.Headers.Add("Authorization", string.Concat(AuthenticationInfo.TokenType, " ", AuthenticationInfo.AccessToken));
        }

        public byte[] DownloadSecureData(string url)
        {
            var result = new byte[0];

            result = base.DownloadData(url);

            return result;
        }

        public byte[] UploadSecureData(string url, dynamic postData)
        {
            var result = new byte[0];

            NameValueCollection reqparm = GetNameValueCollection(postData);

            CreateSignatureHeader(url, "POST", reqparm);

            result = base.UploadValues(url, "POST", reqparm);

            return result;

        }

        public void CreateLogin(string url, string username, string password)
        {

            var reqparm = new NameValueCollection() {
                { "grant_type", "password" },
                { "Email", username },
                { "ConfirmPassword", password },
                { "Password", password },
                { "contentType", "application/json; charset=utf-8" } };

            byte[] responsebytes = base.UploadValues(url, "POST", reqparm);
        }

        private AuthenticationResponse ExtractAccessToken(byte[] response)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AuthenticationResponse));

            MemoryStream ms = new MemoryStream(response);
            AuthenticationResponse result = serializer.ReadObject(ms) as AuthenticationResponse;

            return result;
        }

        private NameValueCollection GetNameValueCollection(object o)
        {
            var result = new NameValueCollection();

            var type = o.GetType();

            type.GetProperties().ToList().ForEach(x =>
            {
                result.Add(x.Name, x.GetValue(o).ToString());
            });

            return result;
        }

        private void CreateSignatureHeader(string url, string method, NameValueCollection reqparm)
        {
            var calculator = new ApiKeyCalculator();
            base.Headers.Remove("amx");
            base.Headers.Add("amx", calculator.CalculateHmacHash(url, method, reqparm, APPId, APIKey));
        }

    }
}