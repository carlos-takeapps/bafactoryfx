using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.MVCExtension
{
    public class MembershipSessionIdentifier
    {
        private string sessionToken;

        public dynamic CustomSessionInfo { get; set; }

        public long UserId { get; set; }
        public long OrganizationId { get; set; }
        public long UserOrganizationId { get; set; }
        public string SessionToken
        {
            get { return sessionToken; }
            private set { sessionToken = value; }
        }

        public MembershipSessionIdentifier()
        {
            this.UserId = 0;
            this.OrganizationId = 0;
            this.SessionToken = null;
        }

        public MembershipSessionIdentifier(long userId, long organizationId, long userOrganizationId)
        {
            this.UserId = userId;
            this.OrganizationId = organizationId;
            this.UserOrganizationId = userOrganizationId;
            this.SessionToken = Guid.NewGuid().ToString();
        }

        public string StrongName
        {
            get { return string.Format("{0}:{1}:{2}", UserId, OrganizationId, SessionToken); }
            private set { return; }
        }
    }
}