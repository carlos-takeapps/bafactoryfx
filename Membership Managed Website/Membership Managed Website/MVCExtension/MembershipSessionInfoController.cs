using System.Web.Mvc;

namespace BAFactory.Fx.Security.MVCExtension
{
    public abstract class MembershipSessionInfoController : Controller
    {
        public MembershipSessionInfoController()
            : base()
        { }

        protected MembershipSessionIdentifier SessionIdentification
        {
            get
            {
                return MembershipSessionDataProvider.SessionIdentification;
            }
            set
            {
                MembershipSessionDataProvider.SessionIdentification = value;
            }
        }

    }
}