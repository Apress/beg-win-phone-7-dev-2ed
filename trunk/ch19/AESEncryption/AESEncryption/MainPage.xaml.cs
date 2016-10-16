using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace AESEncryption
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public string Encrypt(string dataToEncrypt, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memStream = null;
            CryptoStream crStream = null;

            try
            {
                //Generate a Key based on a Password and Salt
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));

                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                memStream = new MemoryStream();
                crStream = new CryptoStream(memStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
                crStream.Write(data, 0, data.Length);
                crStream.FlushFinalBlock();

                //Return Base 64 String 
                return Convert.ToBase64String(memStream.ToArray());
            }
            finally
            {
                //cleanup
                if (crStream != null)
                    crStream.Close();

                if (memStream != null)
                    memStream.Close();

                if (aes != null)
                    aes.Clear();
            }
        }

        public string Decrypt(string dataToDecrypt, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memStream = null;
            CryptoStream crStream = null;

            try
            {
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                memStream = new MemoryStream();
                crStream = new CryptoStream(memStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                byte[] data = Convert.FromBase64String(dataToDecrypt);
                crStream.Write(data, 0, data.Length);
                crStream.FlushFinalBlock();

                byte[] decryptBytes = memStream.ToArray();
                return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }
            finally
            {
                if (crStream != null)
                    crStream.Close();

                if (memStream != null)
                    memStream.Close();

                if (aes != null)
                    aes.Clear();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            txtDecryptedData.Text = Decrypt(txtEncryptedData.Text, txtPassword.Text, txtSalt.Text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            txtEncryptedData.Text = Encrypt(txtDataToEncrypt.Text, txtPassword.Text, txtSalt.Text);
        }

    }
}