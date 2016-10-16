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

namespace HMACTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string message = txtMessage.Text;
            string key = txtKey.Text;

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            byte[] keyByte = encoding.GetBytes(key);

            SHA1Managed sha1 = new SHA1Managed();

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
            textBlock1.Text = ConvertToString(hashmessage);

            hashmessage = hmacsha256.ComputeHash(messageBytes);
            textBlock2.Text = ConvertToString(hashmessage);

        }

        public static string ConvertToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                //hex-formatted
                sbinary += buff[i].ToString("X2");
            }
            return (sbinary);
        }

    }
}