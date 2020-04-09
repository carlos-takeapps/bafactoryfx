using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BAFactory.Fx.Security.Cryptography
{
    public abstract class RijndaelEncryptorBase
    {
        protected static string baseInitString;

        protected static byte[] Encrypt(string input, byte[] key, byte[] IV)
        {
            if (input == null || input.Length <= 0)
            {
                return new byte[0];
            }

            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;

            RijndaelManaged aesAlg = null;

            byte[] encrypted = null;

            try
            {
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);

                swEncrypt.Write(input);
            }
            finally
            {
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();

                if (aesAlg != null)
                    aesAlg.Clear();
            }

            encrypted = msEncrypt.ToArray();
            return encrypted;
        }

        protected static string Decrypt(byte[] input, byte[] key, byte[] IV)
        {
            if (input == null || input.Length <= 0)
            {
                return string.Empty;
            }

            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            RijndaelManaged aesAlg = null;

            string plaintext = null;

            try
            {
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                msDecrypt = new MemoryStream(input);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                plaintext = srDecrypt.ReadToEnd();
            }
            finally
            {
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        protected static string StringSelfPadding(string Input, int Lenght)
        {
            string padded = string.Empty;

            while (padded.Length < Lenght)
            {
                padded += Input;
            }

            return padded.Substring(0, Lenght);
        }
    }
}
