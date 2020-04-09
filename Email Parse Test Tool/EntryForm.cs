using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Email_Parse_Test_Tool
{
    public partial class EntryForm : Form
    {
        public EntryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 parsingForm = new Form1();
            parsingForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 connectionForm = new Form2();
            connectionForm.ShowDialog();
        }
    }
}
