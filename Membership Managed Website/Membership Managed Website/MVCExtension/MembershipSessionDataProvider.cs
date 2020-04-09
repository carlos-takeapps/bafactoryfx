using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.Areas.Membership.Extensions;

namespace BAFactory.Fx.Security.MVCExtension
{
    public static class MembershipSessionDataProvider
    {
        public static HttpSessionState RequestSession { get; set; }

        public static User SessionUser
        {
            get
            {
                return GetSessionObject<User>(MembershipSessionDataKeys.User);
            }
            set
            {
                SetSessionObject<User>(MembershipSessionDataKeys.User, value);
            }
        }

        public static Organization SessionOrganization
        {
            get
            {
                return GetSessionObject<Organization>(MembershipSessionDataKeys.Organization);
            }
            set
            {
                SetSessionObject<Organization>(MembershipSessionDataKeys.Organization, value);
            }
        }

        public static UserOrganization SessionUserOrganization
        {
            get
            {
                return GetSessionObject<UserOrganization>(MembershipSessionDataKeys.UserOrganization);
            }
            set
            {
                SetSessionObject<UserOrganization>(MembershipSessionDataKeys.UserOrganization, value);
            }
        }

        public static MembershipSessionIdentifier SessionIdentification
        {
            get
            {
                return GetSessionObject<MembershipSessionIdentifier>(MembershipSessionDataKeys.SessionIdentifier);
            }
            set
            {
                SetSessionObject<MembershipSessionIdentifier>(MembershipSessionDataKeys.SessionIdentifier, value);
            }
        }

        public static FilterInformation FilterInformation
        {
            get
            {
                return GetSessionObject<FilterInformation>(MembershipSessionDataKeys.Filter);
            }
            set
            {
                SetSessionObject<FilterInformation>(MembershipSessionDataKeys.Filter, value);
            }
        }

        public static void SetSessionObject<T>(string key, T o)
        {
            if (!string.IsNullOrEmpty(key) && 
                RequestSession != null)
            {
                RequestSession[key] = o;
            }
        }

        public static T GetSessionObject<T>(string key)
        {
            T result = Activator.CreateInstance<T>();

            if (RequestSession != null)
            {
                if (RequestSession.Keys.Cast<string>().Contains(key))
                {
                    result = (T)RequestSession[key];
                }
            }

            return result;
        }
    }
}