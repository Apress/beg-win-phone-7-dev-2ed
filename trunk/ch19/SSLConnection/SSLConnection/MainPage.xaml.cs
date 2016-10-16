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
using System.IO;

namespace SSLConnection
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(HandleResponse);
            client.OpenReadAsync(new Uri("https://wp7server/index.html"));
        }

        void HandleResponse(object sender, OpenReadCompletedEventArgs e)
        {
            StreamReader reader = new StreamReader(e.Result);
            string res = reader.ReadToEnd();

            webBrowser1.NavigateToString(res);
        }
    }
}