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
using Microsoft.Phone.Reactive;
using System.Threading;
using System.Diagnostics;

namespace FlickrRx
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var keys =  Observable.FromEvent<KeyEventArgs>(txtSearchTerms, "KeyUp").Throttle(TimeSpan.FromSeconds(.5)).DistinctUntilChanged();
            
            keys.ObserveOn(Deployment.Current.Dispatcher).Subscribe(evt =>
            {
                if (txtSearchTerms.Text.Length > 0)
                {
                    lblSearchingFor.Text = "Searching for ..." + txtSearchTerms.Text;
                    lblLoading.Visibility=System.Windows.Visibility.Visible;
                    loadingImages.Begin();

                    webResults.Navigate(new Uri("http://www.flickr.com/search/?q=" + txtSearchTerms.Text));
                }
            });

            var browser =  Observable.FromEvent<System.Windows.Navigation.NavigationEventArgs>(webResults, "Navigated");

            browser.ObserveOn(Deployment.Current.Dispatcher).Subscribe(evt =>
            {
                loadingImages.Stop();
                lblLoading.Visibility = System.Windows.Visibility.Collapsed;
            });
        }
    }
}
