using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailMessage
    {
        private EMailAddress[] cc;
        private EMailAddress from;
        private string header;
        private EMailBodyAlternateView body;
        private string priority;
        private EMailAddress replyTo;
        private EMailAddress sender;
        private string subject;
        private string date;
        private EMailAddress[] to;
        private string contentClasses;
        private string contentTransferEncoding;
        private string mimeVersion;
        private string contentType;
        private OtherHeadersCollection otherHeaders;
        private EMailBodyAlternateView[] views;
        private EMailAttachment[] attachments;
        private string subjectEncoding;
        private string bodyEncoding;

        public EMailAddress[] CC
        {
            get { return cc; }
            set
            {
                bool valid = true;
                foreach (EMailAddress email in value)
                {
                    if (!EMailAddress.ValidateAddress(email.Address))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    cc = value;
                }
            }
        }
        public EMailAddress From
        {
            get { return from; }
            set
            {
                if (EMailAddress.ValidateAddress(value.Address))
                { from = value; }
            }
        }
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        public EMailBodyAlternateView Body
        {
            get { return body; }
            set { body = value; }
        }
        public string Priority
        {
            get
            {
                if (priority == null || priority == string.Empty)
                {
                    return "Normal";
                }
                else { return priority; }
            }
            set
            {
                if (value == null || value == string.Empty)
                {
                    priority = "Normal";
                }
                else
                {
                    priority = value;
                };
            }
        }
        public EMailAddress ReplyTo
        {
            get { return replyTo; }
            set
            {
                if (EMailAddress.ValidateAddress(value.Address))
                { replyTo = value; }
            }
        }
        public EMailAddress Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        public EMailAddress[] To
        {
            get { return to; }
            set
            {
                bool valid = true;
                foreach (EMailAddress email in value)
                {
                    if (!EMailAddress.ValidateAddress(email.Address))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    to = value;
                }
            }
        }
        public string ContentClasses
        {
            get { return contentClasses; }
            set { contentClasses = value; }
        }
        public string MIMEVersion
        {
            get { return mimeVersion; }
            set { mimeVersion = value; }
        }
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
        public OtherHeadersCollection OtherHeaders
        {
            get { return otherHeaders; }
            set { otherHeaders = value; }
        }
        public EMailBodyAlternateView[] Views
        {
            get { return views; }
            set { views = value; }
        }
        public EMailAttachment[] Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }
        public string SubjectEncoding
        {
            get { return subjectEncoding; }
            set { subjectEncoding = value; }
        }
        public string BodyEncoding
        {
            get { return bodyEncoding; }
            set { bodyEncoding = value; }
        }

        private int number;
        private long bytes;
        private bool retrieved;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public long Bytes
        {
            get { return bytes; }
            set { bytes = value; }
        }
        public bool Retrieved
        {
            get { return retrieved; }
            set { retrieved = value; }
        }

        public EMailMessage()
            : this(new EMailsListElement())
        {
        }
        public EMailMessage(EMailsListElement ListElement)
        {
            cc = new EMailAddress[0];
            EMailAddress from = new EMailAddress();
            header = string.Empty;
            body = new EMailBodyAlternateView();
            priority = "Normal";
            replyTo = new EMailAddress();
            sender = new EMailAddress();
            subject = string.Empty;
            date = DateTime.Now.ToShortDateString();
            to = new EMailAddress[0];
            contentClasses = string.Empty;
            mimeVersion = string.Empty;
            contentType = string.Empty;
            otherHeaders = new OtherHeadersCollection();
            views = new EMailBodyAlternateView[0];
            attachments = new EMailAttachment[0];
            subjectEncoding = string.Empty;
            bodyEncoding = string.Empty;

            number = ListElement.Number;
            bytes = ListElement.Bytes;
            retrieved = ListElement.Retrieved;
        }

        public static explicit operator MailMessage(EMailMessage XEMailMessage)
        {
            MailMessage translatedMessage = new MailMessage();
            translatedMessage.From = (MailAddress)XEMailMessage.From;

            MailAddressCollection toHandler = translatedMessage.To;

            foreach (EMailAddress address in XEMailMessage.to)
            {
                toHandler.Add((MailAddress)address);
            }

            translatedMessage.Subject = XEMailMessage.Subject;
            translatedMessage.Priority = (MailPriority)EMailTranslator.ConvertToMailPriorityIndex(XEMailMessage.Priority);
            translatedMessage.Body = XEMailMessage.Body.ContentStream;

            AlternateViewCollection views = translatedMessage.AlternateViews;
            foreach (EMailBodyAlternateView view in XEMailMessage.Views)
            {
                views.Add((AlternateView)view);
            }

            return translatedMessage;
        }
    }
}
