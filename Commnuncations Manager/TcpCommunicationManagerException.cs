using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Network.Sockets
{
    public class TcpCommunicationManagerException : Exception
    {
        public TcpCommunicationManagerException(string Message) : base(Message) { }
    }
}
