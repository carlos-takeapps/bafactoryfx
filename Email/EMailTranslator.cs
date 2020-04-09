using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

using System.Text.RegularExpressions;

namespace BAFactory.Fx.Network.Email
{
    internal static class EMailTranslator
    {
        internal static MailAddressCollection ConvertEMailAddressArrayToMailAddressCollection(EMailAddress[] XEMailAddresses)
        {
            MailAddressCollection transCollection = new MailAddressCollection();
            foreach (EMailAddress mail in XEMailAddresses)
            {
                transCollection.Add((MailAddress)mail);
            }
            return transCollection;
        }

        internal static AlternateViewCollection ConvertEMailBodyAlternateViewArrayToAlternateViewCollection(EMailBodyAlternateView[] Views)
        {
            MailMessage mail = new MailMessage();
            AlternateViewCollection translatedCol = mail.AlternateViews;
            foreach (EMailBodyAlternateView view in Views)
            {
                translatedCol.Add((AlternateView)view);
            }
            return translatedCol;
        }

        internal static int ConvertToMailPriorityIndex(string Priority)
        {
            switch (Priority)
            {
                case "High":
                    return (int)MailPriority.High;
                case "Normal":
                    return (int)MailPriority.Normal;
                case "Low":
                    return (int)MailPriority.Low;
                default:
                    return 1;
            }
        }
    }
}
