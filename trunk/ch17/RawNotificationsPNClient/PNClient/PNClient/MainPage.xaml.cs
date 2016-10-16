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
using Microsoft.Phone.Notification;
using System.Diagnostics;

namespace PNClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        Uri channelUri;

        public Uri ChannelUri
        {
            get { return channelUri; }
            set
            {
                channelUri = value;
                OnChannelUriChanged(value);
            }
        }

        private void OnChannelUriChanged(Uri value)
        {
            Dispatcher.BeginInvoke(() =>
            {
                txtURI.Text = "changing uri to " + value.ToString();
            });

            Debug.WriteLine("changing uri to " + value.ToString());
        }

        public MainPage()
        {
            InitializeComponent();            
        }

        private void btnCreateChannel_Click(object sender, RoutedEventArgs e)
        {
            SetupChannel();
        }

        private void SetupChannel()
        {
            HttpNotificationChannel httpChannel = null;
            string channelName = "DemoChannel";

            try
            {
                //if channel exists, retrieve existing channel
                httpChannel = HttpNotificationChannel.Find(channelName);
                if (httpChannel != null)
                {
                    //If we can't get it, then close and reopen it.
                    if (httpChannel.ChannelUri == null)
                    {
                        httpChannel.UnbindToShellToast();
                        httpChannel.Close();
                        SetupChannel();
                        return;
                    }
                    else
                    {
                        ChannelUri = httpChannel.ChannelUri;
                        txtURI.Text = "Channel retrieved: " + httpChannel.ChannelUri;
                        //wiring up the raw notifications event handler
                        httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);
                    }
                }
                else
                {
                    httpChannel = new HttpNotificationChannel(channelName);
                    httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ExceptionOccurred);

                    //wiring up the raw notifications event handler
                    httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);

                    httpChannel.Open();
                }
            }
            catch (Exception ex)
            {

            }
        }

        void httpChannel_ExceptionOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            //Display Message on error
            Debug.WriteLine ( e.Message);
        }

        void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            //We get the new Uri (or maybe it's updated)
            ChannelUri = e.ChannelUri;
        }

        // Receiving a raw notification. 
        // Raw notifications are only delivered to the application when it is running in the foreground. 
        // If the application is not running in the foreground, the raw notification message is dropped.
        void httpChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            if (e.Notification.Body != null && e.Notification.Headers != null)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body);
                Dispatcher.BeginInvoke(() =>
                {
                    txtURI.Text = "Raw Notification Message Received: " + reader.ReadToEnd();
                });
            }
        }
    }
}
