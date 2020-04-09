using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace BAFactory.Fx.Security.Cryptography
{
    public sealed class SHA1Encryptor
    {
        private SHA1Encryptor()
        {
        }

        public static string Encrypt(string Input)
        {
            SHA1Managed mngd = new SHA1Managed();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Input);
            byte[] hash = mngd.ComputeHash(buffer);
            return System.Text.Encoding.UTF8.GetString(hash);
        }
    }

}
