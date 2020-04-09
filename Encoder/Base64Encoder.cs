using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace BAFactory.Fx.Utilities.Encoding
{
    public class Base64Encoder
    {
        public Base64Encoder()
        { }

        public string Decode(string input)
        {
            return UTF8Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }

        public string Encode(string input)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
        }

        public bool CreateFileFromBase64String(string Base64Input, string FilePath)
        {
            byte[] base64Bytes = Convert.FromBase64String(Base64Input);

            FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Write, FilePath);
            perm.Demand();

            using (FileStream stream = new FileStream(FilePath, FileMode.Create))
            {
                stream.Write(base64Bytes, 0, base64Bytes.Length);
            }

            return true;
        }

        public string CreateBase64StringFromFile(string FilePath)
        {
            string result = string.Empty;

            FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Read, FilePath);
            perm.Demand();

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                result = Convert.ToBase64String(bytes);
            }
            return result;
        }
    }
}
