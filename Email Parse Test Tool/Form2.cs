using System;
using System.Windows.Forms;
using BAFactory.Fx.Network.Email;
using BAFactory.Fx.Utilities.Email;

namespace Email_Parse_Test_Tool
{
    public partial class Form2 : Form
    {

        //Imap4Provider provider;
        Pop3Provider provider;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Imap4Provider provider = EmailProviderFactory.CreateProvider<Imap4Provider>("localhost", 143, "testuser@localhost", "testpass", false);
            //provider = EmailProviderFactory.CreateProvider<Pop3Provider>("localhost", 110, "testuser@localhost", "testpass", false);
            provider = EmailProviderFactory.CreateProvider<Pop3Provider>("mail.siprod.net", 110, "test@siprod.net", "testPass09", false);

            provider.OpenConnection();
            listadoMails.DataSource = provider.GetAllMessagesHeadersDT(0);
            provider.CloseConnection();
        }

        private void listadoMails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Imap4Provider provider = EmailProviderFactory.CreateProvider<Imap4Provider>("localhost", 143, "testuser@localhost", "testpass", false);
            provider = EmailProviderFactory.CreateProvider<Pop3Provider>("mail.siprod.net", 110, "test@siprod.net", "testPass09", false);
            provider.OpenConnection();
            EMailMessage email = provider.RetrieveEmail(4);
            provider.CloseConnection();

            Form f = new Form();
            WebBrowser w = new WebBrowser();
            f.Controls.Add(w);
            w.DocumentText = email.Body.ContentStream;
            f.ShowDialog();
        }
    }
}
