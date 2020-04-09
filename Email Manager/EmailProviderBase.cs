using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using BAFactory.Fx.Network.Email;
using BAFactory.Fx.Network.Sockets;
using System.Text.RegularExpressions;

namespace BAFactory.Fx.Utilities.Email
{
    public enum EMailTableColumns
    {
        Body,
        Bytes,
        From,
        Headers,
        Number,
        Priority,
        ReplyTo,
        Retrieved,
        Date,
        Sender,
        Subject,
        To,
    }

    public abstract class EmailProviderBase
    {
        private TcpCommunicationManager tcpCommMngr;

        protected byte messageEnd;
        protected byte[] channelLineDelimiter;
        protected string channelLineDelimiterText;

        protected string server;
        protected int port;
        protected string username;
        protected string password;
        protected bool ssl;

        internal EmailProviderBase(string server, int port, bool ssl)
            : this(server, port, ssl, null)
        {
        }
        internal EmailProviderBase(string server, int port, bool ssl, byte[] channelLineDelimiter)
        {
            tcpCommMngr = new TcpCommunicationManager(server, port, null, null, channelLineDelimiter);
            this.channelLineDelimiter = channelLineDelimiter;
            this.channelLineDelimiterText = UTF8Encoding.UTF8.GetString(channelLineDelimiter);
        }

        public EMailMessage[] GetAllMessagesHeaders(int topLines)
        {
            List<EMailsListElement> list = new List<EMailsListElement>();
            List<EMailMessage> result = new List<EMailMessage>();

            list.AddRange(ListEmails());

            foreach (EMailsListElement e in list)
            {
                result.Add(GetMessageHeaders(e));
            }

            return result.ToArray();
        }

        public DataTable GetAllMessagesHeadersDT(int TopLines)
        {
            DataTable result = InitializeDataTable();

            EMailMessage[] list = GetAllMessagesHeaders(TopLines);

            foreach (EMailMessage message in list)
            {
                DataRow messageRow = result.NewRow();

                FillDataRowFromMessage(message, ref messageRow);

                result.Rows.Add(messageRow);
            }

            return result;
        }

        public EMailMessage RetrieveEmail(int MailNumber)
        {
            EMailsListElement element = new EMailsListElement();
            element.Number = MailNumber;
            return RetrieveEmail(element);
        }

        public EMailMessage GetMessageHeaders(int MailNumber)
        {
            return GetMessageHeaders(MailNumber, 0);
        }
        public EMailMessage GetMessageHeaders(EMailsListElement Element)
        {
            EMailMessage message = GetMessageHeaders(Element.Number, 0);
            message.Bytes = Element.Bytes;
            return message;
        }

        public bool DeleteMessage(int MessageNumber)
        {
            EMailsListElement element = new EMailsListElement();
            element.Number = MessageNumber;
            return DeleteMessage(element);
        }

        abstract public bool Authenticate();

        private void Connect()
        {
            this.tcpCommMngr.Connect();

            string response = tcpCommMngr.ReadResponse();

            if (!IsResponseOK(response))
            {
                throw new Pop3ProviderException(response);
            }
        }
        protected void Disconnect()
        {
            //if (this.tcpCommMngr.Connected)
            //{
            this.tcpCommMngr.Disconnect();
            //}
        }

        virtual public void OpenConnection()
        {
            if (tcpCommMngr.Connected)
            {
                return;
            }
            Connect();
            Authenticate();
        }
        virtual public void CloseConnection()
        {
            Disconnect();
        }

        protected string ExecuteCommand(string command)
        {
            string response;

            tcpCommMngr.WriteCommand(command);

            response = tcpCommMngr.ReadResponse();

            return response;
        }

        protected string ExecuteCommand(string command, Regex messageEndPattern)
        {
            string response;

            tcpCommMngr.WriteCommand(command);

            response = tcpCommMngr.ReadResponse(messageEndPattern);

            return response;
        }

        protected string[] ExecuteCommands(string[] commands, Regex messageEndPattern)
        {
            string[] response = new string[commands.Length];

            try
            {
                for (int i = 0; i < commands.Length; ++i)
                {
                    response[i] = ExecuteCommand(commands[i], messageEndPattern);
                }
            }
            finally
            {
                tcpCommMngr.Disconnect();
            }

            return response;
        }

        abstract public EMailsListElement[] ListEmails();
        abstract public EMailMessage RetrieveEmail(EMailsListElement ListElement);
        abstract public string RetrieveRawEmailStream(int MailNumber);
        abstract public EMailMessage GetMessageHeaders(int mailNumber, int topLines);
        abstract public bool DeleteMessage(EMailsListElement ListElement);
        abstract public bool IsResponseOK(string Response);

        private DataTable InitializeDataTable()
        {
            DataTable result = new DataTable();

            result.Columns.Add(EMailTableColumns.Body.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Bytes.ToString(), typeof(uint));
            result.Columns.Add(EMailTableColumns.From.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Headers.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Number.ToString(), typeof(uint));
            result.Columns.Add(EMailTableColumns.Priority.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.ReplyTo.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Retrieved.ToString(), typeof(bool));
            result.Columns.Add(EMailTableColumns.Sender.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Subject.ToString(), typeof(string));
            result.Columns.Add(EMailTableColumns.Date.ToString(), typeof(DateTime));
            result.Columns.Add(EMailTableColumns.To.ToString(), typeof(string));

            result.TableName = "EMailDataTable";

            return result;
        }

        private void FillDataRowFromMessage(EMailMessage message, ref DataRow messageRow)
        {
            EmailParser parser = new EmailParser();

            messageRow[EMailTableColumns.Headers.ToString()] = message.Header;

            messageRow[EMailTableColumns.From.ToString()] = (message.From == null ? string.Empty : string.Format("{0} <{1}>", message.From.DisplayName, message.From.Address));
            messageRow[EMailTableColumns.Priority.ToString()] = message.Priority.ToString();
            messageRow[EMailTableColumns.Number.ToString()] = message.Number;
            messageRow[EMailTableColumns.ReplyTo.ToString()] = (message.ReplyTo == null ? string.Empty : string.Format("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address));
            messageRow[EMailTableColumns.Sender.ToString()] = (message.Sender == null ? string.Empty : string.Format("{0} <{1}>", message.Sender.DisplayName, message.Sender.Address));
            messageRow[EMailTableColumns.Subject.ToString()] = message.Subject;
            messageRow[EMailTableColumns.Bytes.ToString()] = message.Bytes;
            messageRow[EMailTableColumns.Retrieved.ToString()] = message.Retrieved;
            try
            {
                messageRow[EMailTableColumns.Date.ToString()] = ((message.Date == null || message.Date == string.Empty) ? DateTime.MinValue.ToShortDateString() : message.Date);
            }
            catch
            {
                messageRow[EMailTableColumns.Date.ToString()] = DateTime.MinValue;
            }

            StringBuilder bldr = new StringBuilder();

            if (message.To != null)
            {
                foreach (EMailAddress to in message.To)
                {
                    // TODO: review EmailParser instanciation
                    bldr.AppendFormat("{0} ", new EmailParser().GetEMailAddressTag(to));
                }
                messageRow[EMailTableColumns.To.ToString()] = bldr.ToString();
            }
        }
    }
}
