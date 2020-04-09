using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Utilities.Email
{
    class Pop3ProviderException : Exception
    {
        public Pop3ProviderException(string Message) : base(Message) { }
    }
}
