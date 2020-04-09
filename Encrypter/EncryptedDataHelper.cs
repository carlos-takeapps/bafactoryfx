using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Security.Cryptography
{
    public static class EncryptedDataHelper
    {
        public static string ConvertToNumbersString(byte[] Input)
        {
            StringBuilder strBldr = new StringBuilder();
            foreach (byte b in Input)
            {
                strBldr.AppendFormat("{0:000}", b);
            }
            return strBldr.ToString();
        }

        public static byte[] ConvertFromNumbersString(string Input)
        {
            byte[] result = new byte[Input.Length / 3];

            for (int i = 0; i < Input.Length / 3; ++i)
            {
                result[i] = byte.Parse(Input.Substring(i * 3, 3));
            }

            return result;
        }
    }
}
