using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAFactory.Fx.Samples.WebApiSecurity.WebApi.AppCode
{
    public class HmacHeader
    {
        public string AppId { get; set; }
        public string Base64Signature { get; set; }
        public string Nonce { get; set; }
        public string TimeStamp { get; set; }

        public HmacHeader(string appId, string base64Signature, string nonce, string timeStamp)
        {
            AppId = appId;
            Base64Signature = base64Signature;
            Nonce = nonce;
            TimeStamp = timeStamp;
        }

        public static HmacHeader Parse(string s)
        {
            var parts = s.Split(':');

            if (parts.Count() != 4) { return null; }

            return new HmacHeader(parts[0], parts[1], parts[2], parts[3]);
        }
    }
}