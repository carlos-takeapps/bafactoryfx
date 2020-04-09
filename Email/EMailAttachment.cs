using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailAttachment
    {
        private string name;
        private string contentTransferEncoding;
        private string contentDescription;
        private string contentType;
        private string contentDisposition;
        private string filename;
        private string id;
        private string nameEncoding;
        private string contentStream;

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
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        public string ContentDisposition
        {
            get { return contentDisposition; }
            set { contentDisposition = value; }
        }
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string ContentStream
        {
            get { return contentStream; }
            set { contentStream = value; }
        }
        public string NameEncoding
        {
            get { return nameEncoding; }
            set { nameEncoding = value; }
        }

        public EMailAttachment()
        { }
    }
}
