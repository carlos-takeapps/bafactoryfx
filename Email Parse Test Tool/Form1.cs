using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAFactory.Fx.Utilities.Email;
using BAFactory.Fx.Network.Email;

namespace Email_Parse_Test_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EMailMessage message = new EMailMessage();
            EmailParser parser = new EmailParser();

            parser.ParseEMailMessage(ref message, textBox1.Text);

            message.ToString();
        }
    }
}
