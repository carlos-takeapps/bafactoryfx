using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Security.Cryptography
{
    public class RijndaelEncryptor : RijndaelEncryptorBase
    {
        private byte[] Key { get; set; }
        private byte[] IV { get; set; }

        public RijndaelEncryptor(byte[] key, byte[] IV)
        {
            Key = key;
            this.IV = IV;
        }

        public byte[] Encrypt(string input)
        {
            return Encrypt(input, Key, IV);
        }

        public string Decrypt(byte[] input)
        {
            return Decrypt(input, Key, IV);
        }
    }
}
