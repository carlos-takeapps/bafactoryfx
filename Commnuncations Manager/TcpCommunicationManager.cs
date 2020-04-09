using System;
using System.Collections;
using System.Data;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace BAFactory.Fx.Network.Sockets
{
    public class TcpCommunicationManager : TcpClient
    {
        private string LINEFEEDSTRING;
        private byte[] LINEFEEDBYTES;

        protected NetworkStream netStream;

        protected string serverName;
        protected string userName;
        protected string password;
        protected int serverPort;

        public string ServerName
        {
            get { return serverName; }
        }
        public int ServerPort
        {
            get { return serverPort; }
        }

        public TcpCommunicationManager(string server, int port, string username, string password, byte[] lineFeed)
            : base()
        {
            this.serverName = server;
            this.userName = username;
            this.password = password;
            this.serverPort = port;

            LINEFEEDBYTES = lineFeed;
            LINEFEEDSTRING = UTF8Encoding.UTF8.GetString(lineFeed);
        }

        ~TcpCommunicationManager()
        {
            this.Disconnect();
        }

        public virtual void Connect()
        {
            if (!base.Connected)
            {
                base.Connect(this.serverName, this.serverPort);
            }

            netStream = GetStream();
        }

        public virtual void Disconnect()
        {
            this.Close();
        }

        public string SendCommand(string command)
        {
            WriteCommand(command);
            return ReadResponse();
        }

        public void WriteCommand(string Command)
        {   
            if (!this.Connected)
            {
                throw new TcpCommunicationManagerException("Connection Lost");
            }

            Command = string.Format("{0}{1}", Command, LINEFEEDSTRING);
            UTF8Encoding en = new UTF8Encoding();
            byte[] WriteBuffer = new byte[1024];
            WriteBuffer = en.GetBytes(Command);

            WriteToStream(WriteBuffer, 0, WriteBuffer.Length);
        }

        public string ReadResponse()
        {
            if (!this.Connected)
            {
                throw new TcpCommunicationManagerException("Connection Lost");
            }

            List<byte> serverBuffer = new List<byte>();
            int bytes = 0;
            byte[] buff = new byte[1];
            byte[] lastBytes = new byte[2];

            do
            {
                bytes = ReadFromStream(ref buff, 0, 1);
                if (bytes == 1)
                {
                    lastBytes[0] = lastBytes[1];
                    lastBytes[1] = buff[0];

                    serverBuffer.Add(buff[0]);
                }
                else
                {
                    break;
                }
            } while (!(lastBytes[0] == LINEFEEDBYTES[0] && lastBytes[1] == LINEFEEDBYTES[1]));

            return UTF8Encoding.UTF8.GetString(serverBuffer.ToArray());
        }

        public string ReadResponse(Regex messageEndPattern)
        {
            if (!this.Connected)
            {
                throw new TcpCommunicationManagerException("Connection Lost");
            }

            List<byte> serverBuffer = new List<byte>();
            int bytes = 0;
            byte[] buff = new byte[1];
            byte[] lastBytes = new byte[2];

            do
            {
                bytes = ReadFromStream(ref buff, 0, 1);
                if (bytes == 1)
                {
                    lastBytes[0] = lastBytes[1];
                    lastBytes[1] = buff[0];

                    serverBuffer.Add(buff[0]);
                }
            } while (!(lastBytes[0].Equals(LINEFEEDBYTES[0]) && lastBytes[1].Equals(LINEFEEDBYTES[1])
                && ResponseEndReceived(serverBuffer, messageEndPattern)));

            return UTF8Encoding.UTF8.GetString(serverBuffer.ToArray());
        }

        private bool ResponseEndReceived(List<byte> serverBuffer, Regex responsePattern)
        {
            string buffered = UTF8Encoding.UTF8.GetString(serverBuffer.ToArray());
            string[] lines = buffered.Split(LINEFEEDSTRING.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            string lastLine = lines[lines.Length - 1];
            return responsePattern.IsMatch(lastLine);
        }

        protected virtual void WriteToStream(byte[] buffer, int start, int length)
        {
            netStream.Write(buffer, start, length);
            netStream.Flush();
        }

        protected virtual int ReadFromStream(ref byte[] buff, int start, int length)
        {
            return netStream.Read(buff, start, length);
        }
    }
}
