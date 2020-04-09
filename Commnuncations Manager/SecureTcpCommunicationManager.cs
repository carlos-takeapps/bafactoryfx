using System.Net.Security;

namespace BAFactory.Fx.Network.Sockets
{
    public class SecureTcpCommunicationManager : TcpCommunicationManager
    {
        private SslStream sslStream;

        public SecureTcpCommunicationManager(string server, int port, string username, string password, byte[] lineFeed)
            : base(server, port, username, password, lineFeed)
        { }

        ~SecureTcpCommunicationManager()
        {
            this.Disconnect();
        }

        public override void Connect()
        {
            sslStream = new SslStream(netStream, true);
            sslStream.AuthenticateAsClient(ServerName);

            base.Connect();
        }

        public override void Disconnect()
        {
            if (sslStream != null)
            {
                sslStream.Close();
            }
            base.Disconnect();
        }

        protected override void WriteToStream(byte[] buffer, int start, int length)
        {
            sslStream.Write(buffer, start, length);
            sslStream.Flush();
        }

        protected override int ReadFromStream(ref byte[] buff, int start, int length)
        {
            int bytes = sslStream.Read(buff, start, length);
            return bytes;
        }

    }
}
