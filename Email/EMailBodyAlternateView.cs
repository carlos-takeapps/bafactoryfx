using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;
using System.Net.Mime;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailBodyAlternateView
    {
        private string contentType;
        private string contentTransferEncoding;
        private string charset;
        private string contentStream;
        private string baseUri;
        private string id;
        private string type;
        private string[] linkedResources;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        public string ContentTransferEncoding
        {
            get { return contentTransferEncoding; }
            set { contentTransferEncoding = value; }
        }
        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }
        public string ContentStream
        {
            get { return contentStream; }
            set { contentStream = value; }
        }
        public string BaseUri
        {
            get { return baseUri; }
            set { baseUri = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string[] LinkedResources
        {
            get { return linkedResources; }
            set { linkedResources = value; }
        }

        public EMailBodyAlternateView()
        { }

        public static explicit operator AlternateView(EMailBodyAlternateView EMailAlternateView)
        {
            AlternateView translated = AlternateView.CreateAlternateViewFromString(EMailAlternateView.ContentStream);

            return translated;
        }

        private static int ConvertToTransferEncodingIndex(string TransferEncodingName)
        {
            switch (TransferEncodingName)
            {
                case "Base64":
                    return (int)TransferEncoding.Base64;
                case "QuotedPrintable":
                    return (int)TransferEncoding.QuotedPrintable;
                case "SevenBit":
                    return (int)TransferEncoding.SevenBit;
                default:
                    return (int)TransferEncoding.Unknown;
            }
        }
    }
}
