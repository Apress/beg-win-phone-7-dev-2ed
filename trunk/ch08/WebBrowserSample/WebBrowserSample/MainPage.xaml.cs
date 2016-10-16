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

namespace WebBrowserSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            webBrowser1.Loaded += new RoutedEventHandler(webBrowser1_Loaded);
        }

        void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser1.Navigate(new Uri("http://www.twitter.com/Microsoft", UriKind.Absolute));
        }
    }
}