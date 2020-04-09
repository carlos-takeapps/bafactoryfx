using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BAFactory.Fx.Utilities.Encoding
{
    public class QuotedPrintableEncoder
    {
        public string EncodeToQuotedPrintable(string Input)
        {
            string result = string.Empty;

            char[] chars = Input.ToCharArray();

            for (int i = 0; i <= chars.Length - 1; i++)
            {
                int ascii = (int)chars[i];
                if (ascii < 32 || ascii == 61 || ascii > 126)
                {
                    string encoded = ascii.ToString("X");
                    if (encoded.Length == 1)
                    {
                        encoded = "0" + encoded.ToCharArray();
                    }
                    result += "=" + encoded;
                }
                else
                {
                    result += chars[i];
                }
            }
            return result;
        }

        public string DecodeFromQuotedPrintable(string Input)
        {
            string result = string.Empty;

            Regex re = new Regex("=\n");
            result = re.Replace(Input, string.Empty);

            re = new Regex("(\\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase);
            result = re.Replace(result, new MatchEvaluator(HexDecoderEvaluator));

            re = new Regex("(&#x([0-9A-F][0-9A-F]));", RegexOptions.IgnoreCase);
            result = re.Replace(result, new MatchEvaluator(HexDecoderEvaluator));

            result = result.Replace("_", " ");

            return result;
        }

        private static string HexDecoderEvaluator(Match m)
        {
            string hex = m.Groups[2].Value;
            int iHex = Convert.ToInt32(hex, 16);
            char c = (char)iHex;
            if (iHex < 32)
            {
                return " ";
            }
            return c.ToString();
        }
    }
}
