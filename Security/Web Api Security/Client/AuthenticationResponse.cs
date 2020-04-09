using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BAFactory.Fx.Samples.WebApiSecurity.Client
{
    [DataContract]
    public class AuthenticationResponse
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = ".issued")]
        public string Issued { get; set; }

        [DataMember(Name = ".expires")]
        public string Expires { get; set; }
    }
}
