using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_KEY_PASS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Click += TextBox1_Click;
            textBox1.KeyDown += TextBox1_KeyDown;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                button2_Click(this, new EventArgs());
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = true;
        }

        private string encryptedContent = "";
        private string file = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString();
            openFileDialog.FilterIndex = 2;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                label1.Text = Path.GetFileName(openFileDialog.FileName);
                file = openFileDialog.FileName;
                var fileStream = openFileDialog.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    encryptedContent = reader.ReadToEnd();
                }
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                Display display = new Display();
                display.Show();
                display.create(encryptedContent, textBox1.Text, file, this);
                Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

        }

    }
}
