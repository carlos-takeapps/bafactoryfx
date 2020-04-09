using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BAFactory.Fx.Network.Email;
using BAFactory.Fx.Network.Sockets;
using System.Text.RegularExpressions;


namespace BAFactory.Fx.Utilities.Email
{
    public sealed class Pop3Provider : EmailProviderBase
    {
        private const string userCommand = "USER {0}";
        private const string passCommand = "PASS {0}";
        private const string listCommand = "LIST";
        private const string retrCommand = "RETR {0}";
        private const string topCommand = "TOP {0} {1}";
        private const string deleCommand = "DELE {0}";
        private const string quitCommand = "QUIT";

        private const string responseOK = "+OK";
        private const string messageEndPattern = @"^\.$";

        public Pop3Provider(string host, int port, string username, string password, bool ssl)
            : base(host, port, ssl, UTF8Encoding.UTF8.GetBytes("\r\n"))
        {
            this.username = username;
            this.password = password;
        }

        override public bool Authenticate()
        {
            string answer = string.Empty;
            string command = string.Format(userCommand, this.username);

            string response = ExecuteCommand(command);

            if (!IsResponseOK(response))
            {
                return false;
            }

            command = string.Format(passCommand, this.password);
            response = ExecuteCommand(command);

            if (!IsResponseOK(response))
            {
                return false;
            }

            return true;
        }

        override public void CloseConnection()
        {
            ExecuteCommand(quitCommand);
            base.CloseConnection();
        }

        override public EMailsListElement[] ListEmails()
        {
            string response = ExecuteCommand(listCommand, new Regex(messageEndPattern));

            string commandResult = string.Empty;
            string[] answerLines = GetAnswer(response, channelLineDelimiter, out commandResult);

            List<EMailsListElement> result = new List<EMailsListElement>();
            ArrayList retval = new ArrayList();
            char[] seps = { ' ' };
            for (int i = 0; i < answerLines.Length; ++i)
            {
                EMailsListElement msg = new EMailsListElement();
                string[] values = answerLines[i].Split(seps);

                if (values.Length < 2) continue;

                msg.Number = Int32.Parse(values[0]);
                msg.Bytes = Int32.Parse(values[1]);
                msg.Retrieved = false;

                result.Add(msg);
            }

            return result.ToArray();
        }

        override public EMailMessage RetrieveEmail(EMailsListElement ListElement)
        {
            string response = RetrieveRawEmailStream(ListElement.Number);

            EmailParser parser = new EmailParser();

            // TODO: clean it up
            Regex rx = new Regex(@"(\+OK \d* Octets\r\n)");
            string answer = rx.Replace(response, string.Empty);

            EMailMessage result = parser.CreateEmailFromRawText(answer);

            result.Retrieved = true;

            return result;
        }

        override public string RetrieveRawEmailStream(int MailNumber)
        {
            string command = string.Format(retrCommand, MailNumber);

            string response = ExecuteCommand(command, new Regex(messageEndPattern));

            return response;
        }

        override public EMailMessage GetMessageHeaders(int mailNumber, int TopLines)
        {
            EMailMessage result;
            string commandResult = string.Empty;

            string command = string.Format(topCommand, mailNumber, TopLines);

            string response = ExecuteCommand(command, new Regex(messageEndPattern));

            string answer = GetAnswer(response, out commandResult);

            EMailsListElement element = new EMailsListElement { Number = mailNumber };
            result = new EMailMessage(element);

            EmailParser parser = new EmailParser();
            parser.ParseEMailMessageHeader(ref result, answer);

            return result;
        }

        override public bool DeleteMessage(EMailsListElement ListElement)   
        {
            bool result = false;

            string command = string.Format(deleCommand, ListElement.Number);

            string response = ExecuteCommand(command);

            if (IsResponseOK(response))
            {
                result = true;
            }

            return result;
        }

        override public bool IsResponseOK(string response)
        {
            if (response.Length >= responseOK.Length
                && response.Substring(0, responseOK.Length) == responseOK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string[] GetAnswer(string response, byte[] delimiter, out string commandResult)
        {
            string[] delimiterText = GetString(delimiter);

            List<string> lines = new List<string>();
            lines.AddRange(response.Split(delimiterText, StringSplitOptions.RemoveEmptyEntries));
            commandResult = lines[0];
            lines.RemoveAt(0);
            lines.RemoveAll(x => (x.Trim() == string.Empty || x.Trim() == "."));
            return lines.ToArray();
        }

        private string GetAnswer(string response, out string commandResult)
        {
            int cmdReultIdx = response.IndexOf(channelLineDelimiterText);
            commandResult = response.Substring(0, cmdReultIdx);

            return response.Substring(cmdReultIdx + 2);
        }

        private string[] GetString(byte[] b)
        {
            List<string> result = new List<string>();
            foreach (byte c in b)
            {
                result.Add(UTF8Encoding.UTF8.GetString(new byte[] { c }));
            }
            return result.ToArray();
        }
    }
}
