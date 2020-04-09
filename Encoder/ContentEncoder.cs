using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BAFactory.Fx.Utilities.Encoding
{
    public class ContentEncoder
    {
        public ContentEncoder()
        { }

        public bool IsEncoded(string Input)
        {
            bool result = false;

            Regex splitPatter = new Regex(@"(\=\?.*?\?\=)");
            result = splitPatter.IsMatch(Input);

            return result;
        }

        public string StripEncodedString(string Input, out string CharacterSet, out string TrasferEncoding)
        {
            CharacterSet = string.Empty;
            TrasferEncoding = string.Empty;

            string result = string.Empty;

            Regex rxQuotedPrintable = new Regex(@"(\=\?(.*)\?([qb])\?(.*)\?=)?", RegexOptions.IgnoreCase);
            Match mQuotedPrintable;

            mQuotedPrintable = rxQuotedPrintable.Match(Input);

            if (mQuotedPrintable.Groups.Count > 2)
            {
                result = mQuotedPrintable.Groups[4].Value;
                TrasferEncoding = mQuotedPrintable.Groups[3].Value.ToUpper();
                CharacterSet = mQuotedPrintable.Groups[2].Value;
            }
            else
            {
                result = Input;
            }

            return result;

        }

        public string[] SplitEncodedString(string Input)
        {
            string[] result = new string[0];
            string[] temp = new string[0];

            string[] spaced = Input.Split(' ');

            foreach (string s in spaced)
            {
                Regex splitPatter = new Regex(@"(\=\?.*\?.\?.*?\?\=)");
                MatchCollection matches = splitPatter.Matches(s);

                temp = new string[result.Length + matches.Count];

                result.CopyTo(temp, 0);

                int i = result.Length;
                foreach (Match match in matches)
                {
                    temp[i++] = match.Value;
                }

                result = new string[temp.Length];
                temp.CopyTo(result, 0);

            }
            return result;
            
        }

        public string GetUTF8DecodedText(string InputText)
        {
            StringBuilder result = new StringBuilder();

            if (IsEncoded(InputText))
            {
                string[] parts = SplitEncodedString(InputText);

                foreach (string part in parts)
                {

                    result.Append(DecodeString(part));
                }
            }
            else
            {
                result.Append(InputText);
            }
            return result.ToString();
        }

        
        private string DecodeString(string t)
        {
            string result = string.Empty;

            string charSet = string.Empty;
            string trxEncoding = string.Empty;

            string text = StripEncodedString(t, out charSet, out trxEncoding);

            switch (trxEncoding.ToUpper())
            {
                case "B":
                    Base64Encoder b64Enc = new Base64Encoder();
                    result = b64Enc.Decode(text);
                    break;
                case "Q":
                    QuotedPrintableEncoder qpEnc = new QuotedPrintableEncoder();
                    result = qpEnc.DecodeFromQuotedPrintable(text);
                    break;
                default:
                    result = text;
                    break;
            }

            if (!string.IsNullOrEmpty(charSet))
            {
                System.Text.Encoding enc = null;
                try
                {
                    enc = System.Text.Encoding.GetEncoding(charSet);
                }
                catch
                {
                    return result;
                }
                byte[] decoded = System.Text.Encoding.Convert(enc, new UTF8Encoding(), enc.GetBytes(result));
                result = UTF8Encoding.UTF8.GetString(decoded);
            }

            return result;
        }
    }
}
