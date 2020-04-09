using System;
using BAFactory.Fx.Network.Email;
using BAFactory.Fx.Network.Sockets;
using System.Collections;

namespace BAFactory.Fx.Utilities.Email
{
    public class Imap4Provider : EmailProviderBase
    {
        private const string loginCommand = ". login {0} {1}";
        private const string listCommand = ". list \"{0}\" \"{1}\"";
        private const string statusCommand = ". status {0} ({1})";
        private const string examineCommand = ". examine {0}";
        private const string selectCommand = ". select {0}";
        private const string createCommand = ". create {0}";
        private const string deleteCommand = ". delete {0}";
        private const string renameCommand = ". rename {0} ({1})";
        private const string fetchCommand = ". fetch {0} ({1})";

        private const string responseOK = " OK";

        public Imap4Provider(string Server, int Port, string Username, string Password)
            : this(Server, Port, Username, Password, false)
        { }
        public Imap4Provider(string Server, int Port, string Username, string Password, bool Ssl)
            : base(Server, Port, Username, Password, Ssl)
        { }

        override public bool Authenticate()
        {
            string answer = string.Empty;
            string command = string.Format(loginCommand, this.username, this.password);
            string response = ExecuteCommand(command, true, ref answer, true);

            return (!IsResponseOK(response));
        }
        override public EMailsListElement[] ListEmails(bool keepOpen)
        {
            EMailsListElement[] result;
            string answer = string.Empty;

            OpenConnection();

            string response = ExecuteCommand(listCommand, false, ref answer, true);

            if (!keepOpen) CloseConnection();

            string[] answerLines = answer.Split(responseEndLine);

            ArrayList retval = new ArrayList();
            char[] seps = { ' ' };
            for (int i = 0; i < answerLines.Length - 1; ++i)
            {
                EMailsListElement msg = new EMailsListElement();
                string[] values = answerLines[i].Split(seps);

                if (values.Length < 2) continue;

                msg.Number = Int32.Parse(values[0]);
                msg.Bytes = Int32.Parse(values[1]);
                msg.Retrieved = false;

                retval.Add(msg);
            }

            result = new EMailsListElement[retval.Count];
            result = (EMailsListElement[])(retval.ToArray(typeof(EMailsListElement)));

            return result;
        }
        override public EMailMessage RetrieveEmail(EMailsListElement ListElement)
        {
            throw new NotImplementedException();
        }
        override public string RetrieveRawEmailStream(int MailNumber, out string answer)
        {
            throw new NotImplementedException();
        }
        override public EMailMessage GetMessageHeaders(EMailsListElement Element, int TopLines, bool keepOpen)
        {
            throw new NotImplementedException();
        }
        override public bool DeleteMessage(EMailsListElement ListElement)
        {
            throw new NotImplementedException();
        }

        override protected bool IsResponseOK(string Response)
        {
            if (Response.Substring(1, 3) == responseOK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
