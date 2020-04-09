using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailBodyPart
    {
        private string contentType;
        private string name;
        private string contentTransferEncoding;
        private string contentDescription;
        private string contentDisposition;
        private string filename;
        private string contentID;
        private string charset;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string ContentTransferEncoding
        {
            get { return contentTransferEncoding; }
            set { contentTransferEncoding = value; }
        }
        public string ContentDescription
        {
            get { return contentDescription; }
            set { contentDescription = value; }
        }
        public string ContentDisposition
        {
            get { return contentDisposition; }
            set { contentDisposition = value; }
        }
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        public string ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }
        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }


        public EMailBodyPart()
        { }
    }
}
