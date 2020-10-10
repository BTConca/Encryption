using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace SimpleEncrypt
{
    public partial class Form1 : Form
    {
        private AES _aes;
        //private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
        //private int BlockSize = 128;

        public Form1()
        {

            InitializeComponent();
            _aes = new AES();
            label3.Hide();  // For Dl 2.2
            textBox1.Hide(); // For DL 2.2
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            textBoxFilename.Text = openFileDialog1.FileName;

            FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            richTextBox1.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();
        }


        private void Encrypt_Click(object sender, EventArgs e)
        {
            if (textBoxFilename.Text != "")
            {

                if (textBoxPassword.Text != "")
                {

                    string password = textBoxPassword.Text;
                    string notEncryptedText = richTextBox1.Text;
                    string encryptedText = _aes.Encrypt(password, notEncryptedText);
                    textBox1.Text = _aes.getHashKey();
                    richTextBox1.Text = encryptedText;
                    using (StreamWriter writer = new StreamWriter(textBoxFilename.Text, false))
                    {
                        writer.Write(encryptedText);
                    }
                    if (MessageBox.Show("Encryption is successful, do you want to open file?", "What is your action?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Process.Start(textBoxFilename.Text);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please enter your password!");
                }
            }
            else System.Windows.Forms.MessageBox.Show("Please choose you file!");
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {

            string password = textBoxPassword.Text;
            string decryptingText = richTextBox1.Text;
            string plainText = _aes.Decrypt(password, decryptingText);
            if (decryptingText == "")
            {
                System.Windows.Forms.MessageBox.Show("There is nothing to decrypt!");
            }
            else if (plainText == "")
            {
                System.Windows.Forms.MessageBox.Show("Incorrect Password");
            }
            else if (plainText == null)
            {
                System.Windows.Forms.MessageBox.Show("This is not a encrypted file!");
            }
            else
            {
                richTextBox1.Text = plainText;
                using (StreamWriter writer = new StreamWriter(textBoxFilename.Text, false))
                {
                    writer.Write(plainText);
                }
                if (MessageBox.Show("Decryption is successful, do you want to open file?", "What is your action?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(textBoxFilename.Text);
                }
            }
        }
    }
}
