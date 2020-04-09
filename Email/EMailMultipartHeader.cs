using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailMultipartHeader
    {
        private string contentType;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public EMailMultipartHeader()
        { }
    }
}
