using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Utilities.Encoding
{
    public class EncodingException : Exception
    {
        private string originalText;
        private string encoding;

        public string OriginalText
        {
            get { return originalText; }
            set { originalText = value; }
        }

        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public EncodingException(string Message, string OriginalText, string Encoding): base(Message)
        {
            originalText = OriginalText;
            encoding = Encoding;
        }
    }
}
