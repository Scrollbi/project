using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void button_enter_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("add");
            form.ShowDialog();
        }

        private void button_reg_Click(object sender, EventArgs e)
        {
            AuthForm form = new AuthForm("reg");
            form.ShowDialog();
        }
    }
}
