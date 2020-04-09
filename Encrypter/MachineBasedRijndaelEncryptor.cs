using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BAFactory.Fx.Security.Cryptography
{
    public sealed class MachineBasedRijndaelEncryptor : RijndaelEncryptorBase
    {
        private MachineBasedRijndaelEncryptor() { }

        static MachineBasedRijndaelEncryptor()
        {
            string bytesString = string.Concat(System.Environment.UserDomainName, System.Environment.UserName, System.Environment.MachineName);
            if (bytesString.Length != 32)
            {
                bytesString = StringSelfPadding(bytesString, 32);
            }
            if (bytesString.Length < 1)
            {
                throw new System.Security.Cryptography.CryptographicException("Can not create a base Key for this use. Not enough information.");
            }
            baseInitString = bytesString;
        }

        public static byte[] Encrypt(string input)
        {
            byte[] key, IV;
            GetKeys(out key, out IV);
            
            return Encrypt(input, key, IV);
        }

        public static string Decrypt(byte[] input)
        {
            byte[] key, IV;
            GetKeys(out key, out IV);

            return Decrypt(input, key, IV);
        }

        private static void GetKeys(out byte[] key, out byte[] IV)
        {
            key = System.Text.Encoding.UTF8.GetBytes(StringSelfPadding(baseInitString, 32));
            IV = System.Text.Encoding.UTF8.GetBytes(StringSelfPadding(baseInitString, 16));
        }
    }
}
