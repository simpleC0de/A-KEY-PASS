using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_KEY_PASS
{
    public partial class Display : Form
    {

        private CryptoHandler cHandler = new CryptoHandler();
        private string file = "";
        private string encryptionKey;
        private string superkey = "_sW#Jc4cXaD%FjqUMkLcbyNRXm5-asZGJjKwx6tWeaDn$-U4_xpUYmrE+L_2w2N*9_3Nd_+jN%qD5Z?G$u+EaS$_E_StbGHRBJrcdrt3qSHb+Azpb-jKQLbR7xWH2mASL#C3Bm##&^M6PnWrSS-#jGF&GUfNCf6XFgmHMbWMjZ!bW8HN49FmncW?@j?j_gPc-m@WTm7y9gnq#XkL4ck=&HQrRs-Wnn@FPFux?z4E-c9?4K669Ky9_AM8kaDT@2Wu";


        // never change this, since this is the original value!
        private List<handledPair> pairs = new List<handledPair>();

        public Display()
        {
            InitializeComponent();
        }

        private void Display_Load(object sender, EventArgs e)
        {
            this.FormClosed += Display_FormClosed;
            textBox1.Click += TextBox1_Click;



        }

        private void Display_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        public void create(string content, string key, string filePath, Form1 mainForm)
        {
            
            file = filePath;
            encryptionKey = key;
            try { content = cHandler.Decrypt(content, superkey); }catch(Exception) { }
            try
            {
                
                string[] lines = content.Split('\n');
                foreach (string line in lines)
                {
                    if(line.Length > 12)
                    {
                        string sortedLine = Regex.Replace(line, @"\s+", "");
                        handledPair pair = new handledPair();
                        string decrypted = cHandler.Decrypt(sortedLine, encryptionKey);
                        string[] split = decrypted.Split(new string[] { "<->" }, StringSplitOptions.None);
                        pair.website = split[0];
                        pair.username = split[1];
                        pair.password = split[2];
                        pairs.Add(pair);
                    }
                    

                }
                dataGridView1.DataSource = pairs;
            }
            catch(Exception) {
                MessageBox.Show("Failed decrypting!\nMay be the wrong password..");
                this.Close();
            }
            

        }

        class handledPair
        {
            public string website { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (New_Entry form = new New_Entry())
            {
                var result = form.ShowDialog();
                if(result == DialogResult.OK)
                {
                    string website, username, password;
                    website = form.website;
                    username = form.username;
                    password = form.password;

                    handledPair addition = new handledPair();
                    addition.website = website;
                    addition.username = username;
                    addition.password = password;
                    pairs.Add(addition);
                }
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = pairs;
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                List<string> encryptedLines = new List<string>();
                foreach (handledPair pair in pairs)
                {
                    string encryptedLine;
                    string plainLine = pair.website + "<->" + pair.username + "<->" + pair.password;
                    encryptedLine = cHandler.Encrypt(plainLine, encryptionKey);
                    encryptedLines.Add(encryptedLine);
                }

                string content = "";
                foreach (string str in encryptedLines)
                {
                    content = content + str + "\n";
                }


                string encryptedContent = cHandler.Encrypt(content, superkey);

                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    writer.Write(content);
                }
                MessageBox.Show("Successfully saved..");
                System.GC.Collect();
            });
            t.IsBackground = true;
            t.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string website = (string)dataGridView1.CurrentRow.Cells[0].Value;
            string username = (string)dataGridView1.CurrentRow.Cells[1].Value;
            string password = (string)dataGridView1.CurrentRow.Cells[2].Value;
            foreach(handledPair pair in pairs.ToList())
            {
                if(pair.website == website && pair.username == username && pair.password == password)
                {
                    pairs.Remove(pair);
                }
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = pairs;
            dataGridView1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<handledPair> searchPairs = pairs.ToList();

            foreach(handledPair pair in searchPairs.ToList())
            {
                if(pair.website.ToLower().Contains(textBox1.Text.ToLower()) | pair.username.ToLower().Contains(textBox1.Text.ToLower()) | pair.password.ToLower().Contains(textBox1.Text.ToLower()))
                {
                    continue;
                }
                searchPairs.Remove(pair);
            }
            dataGridView1.DataSource = searchPairs;
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 && e.RowIndex > -1)
                {
                    // Clicks on the website tab
                    string content = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    Process.Start(content);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occured...\nLive with it or restart application");
            }
        }

    }

    //double encrypted superkey
    ///7IVriOv1TYyOI8r74uo9JTj9jcr+ze1K4422S2jAEHZnJpYIApYtJyJ7pR6eBJtpeb0M4E8JgM7QghmnfRG+DfNlkxjNPtfKiAj2tRvZMqTBwz2Ju7m5l4IIiD2ylownjkkG10BQlLkZbnIrqkwXrHK+PnR9jm/Ee8AMXoQRWB0rpTeqsNpK2pygPd0g4II9beYimH3ooUDRRp94cqutkB1XivR/EENhOjI+xcR5xUuhBzsheiLdFNDhv43qcmbHdm7Dcdj7vsanU9ODLASWeUDjGlCxyDuO5+2zB3m22p/FBiSgxGA5DVeIh0gCVXI19ruhIaR91q3wW8IWPLTge+ga5v8jh9XCDEpWBCgldb1wvRIfsE2Fty32ddiqT81e0m9QCISEVsgcYZe8CT3TRsKlHWZNTJUTyX57VoRYoPBsuKpe77WRcHLuWpeqIHjWTU4Nm/auI3ubtMnxDiqDg==
}
