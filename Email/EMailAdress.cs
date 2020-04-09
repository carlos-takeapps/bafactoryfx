using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Mail;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailAddress
    {
        private string user;
        private string host;
        private string displayName;

        public string Username
        {
            get { return user; }
        }
        public string Host
        {
            get { return host; }
        }
        public string DisplayName
        {
            get
            {
                if (displayName == null || displayName == string.Empty)
                { return Address; }
                else
                { return displayName; }
            }
            set { displayName = value; }
        }
        public string Address
        {
            get { return string.Format("{0}@{1}", user, host); }
            set { if (ValidateAddress(value)) { string[] mailParts = value.Split("@".ToCharArray()); user = mailParts[0]; host = mailParts[1]; } }
        }

        public EMailAddress()
        {
        }
        public EMailAddress(string Address)
            : this(Address, string.Empty)
        { }
        public EMailAddress(string Address, string DisplayName)
        {
            if (ValidateAddress(Address))
            {
                string[] mailParts = Address.Split("@".ToCharArray());
                this.user = mailParts[0];
                this.host = mailParts[1];
                this.displayName = DisplayName;
            }
        }

        public string GetEMailAddressTag()
        {
            string result = string.Empty;
            if ((this.displayName == null || this.displayName == string.Empty) &&
                (this.Address != null || this.Address != string.Empty))
            {
                result = this.Address;
            }
            else
            {
                if (this.Address == null || this.Address == string.Empty)
                {
                    result = this.displayName;
                }
                else
                {
                    result = string.Format("{0} <{1}>", this.displayName, this.Address);
                }
            }
            return result;
        }

        public static bool ValidateAddress(string Address)
        {
            return Regex.IsMatch(Address, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static explicit operator MailAddress(EMailAddress XEMailAddress)
        {
            MailAddress translatedAddress = new MailAddress(XEMailAddress.Address, XEMailAddress.DisplayName);

            return translatedAddress;
        }

        /// <summary>
        /// Returns the email address tag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetEMailAddressTag();
        }
    }
}
