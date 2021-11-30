using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_KEY_PASS
{
    public partial class New_Entry : Form
    {
        public string website { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        
        public New_Entry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            website = textBox1.Text;
            username = textBox2.Text;
            password = textBox3.Text;
            int fillScore = 0;
            if (website.Length > 0)
                fillScore += 1;
            if (username.Length > 0)
                fillScore += 1;
            if (password.Length > 0)
                fillScore += 1;
            if (fillScore < 2)
            {
                MessageBox.Show("Please fill out at least two fields.");
                return;
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void New_Entry_Load(object sender, EventArgs e)
        {
            textBox1.KeyDown += TextBox1_KeyDown;
            textBox2.KeyDown += TextBox2_KeyDown;
            textBox3.KeyDown += TextBox3_KeyDown;
        }

        private void TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(this, new EventArgs());
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox3.Focus();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox2.Focus();
        }
    }
}
