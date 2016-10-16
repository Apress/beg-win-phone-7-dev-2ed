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
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.IO;

namespace WindowsPhoneApplication1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            webBrowser1.Loaded += new RoutedEventHandler(webBrowser1_Loaded);
        }

        void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            SaveHelpFileToIsoStore();
            webBrowser1.Navigate(new Uri("Help.htm", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "Ford")
            {
                webBrowser1.NavigateToString(@"<html><head><meta name='viewport' content='width=900, user-scalable=yes' /></head><body><center><div style='font: Arial 12px;'>Which Ford model would you like to see?<br><br>
                    <a href='http://www.bing.com/images/search?q=cars+Ford+Mustang'>Ford Mustang</a> or <a href='http://www.bing.com/images/search?q=cars+Ford+F150'>Ford F-150</a></style></center></body></html>"); 
            }
            else
            {
                webBrowser1.Navigate(new Uri("http://www.bing.com/images/search?q=cars " + textBox1.Text, UriKind.Absolute));
            }
        }

        private void SaveStringToIsoStore(string strWebContent)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //remove the file if exists to allow each run to independently write to
            // the Isolated Storage
            if (isoStore.FileExists("web.htm") == true)
            {
                isoStore.DeleteFile("web.htm");
            }
            StreamResourceInfo sr = new StreamResourceInfo(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strWebContent)), "html/text");
            using (BinaryReader br = new BinaryReader(sr.Stream))
            {
                byte[] data = br.ReadBytes((int)sr.Stream.Length);
                //save file to Isolated Storage
                using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile("web.htm")))
                {
                    bw.Write(data);
                    bw.Close();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string strWebContent = webBrowser1.SaveToString();
            SaveStringToIsoStore(strWebContent);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            webBrowser1.Navigate(new Uri("web.htm", UriKind.Relative));
        }

        private void SaveHelpFileToIsoStore()
        {
            string strFileName = "Help.htm";
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //remove the file if exists to allow each run to independently write to
            // the Isolated Storage
            if (isoStore.FileExists(strFileName) == true)
            {
                isoStore.DeleteFile(strFileName);
            }
            StreamResourceInfo sr = Application.GetResourceStream(new Uri(strFileName, UriKind.Relative));
            using (BinaryReader br = new BinaryReader(sr.Stream))
            {
                byte[] data = br.ReadBytes((int)sr.Stream.Length);
                
                //save file to Isolated Storage
                using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile(strFileName)))
                {
                    bw.Write(data);
                    bw.Close();
                }
            }
        }
    }
}
