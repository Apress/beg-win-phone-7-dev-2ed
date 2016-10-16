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
                //unregister the old URI from the server
                if (channelUri!=null)
                    UnregisterUriFromServer(channelUri);

                //register the new URI with the server
                RegisterUriWithServer(value);

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
                    }
                    BindToShell(httpChannel);
                }
                else
                {

                    httpChannel = new HttpNotificationChannel(channelName);
                    httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ShellToastNotificationReceived+=new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
                    httpChannel.HttpNotificationReceived+=new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ExceptionOccurred);

                    httpChannel.Open();
                    BindToShell(httpChannel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An exception setting up channel " + ex.ToString());
            }
        }

        void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                txtURI.Text = "Toast Notification Message Received: ";
                if (e.Collection != null)   
                {   
                    Dictionary<string, string> collection = (Dictionary<string, string>)e.Collection;   
                    System.Text.StringBuilder messageBuilder = new System.Text.StringBuilder();   
                    foreach (string elementName in collection.Keys)   
                    {
                        txtURI.Text+= string.Format("Key: {0}, Value: {1}\r\n", elementName, collection[elementName]);   
                    }
                }   
            });
        }

        private static void BindToShell(HttpNotificationChannel httpChannel)
        {
            try
            {
                //toast notification binding
                httpChannel.BindToShellToast();
                //tile notification binding
                httpChannel.BindToShellTile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An exception occurred binding to shell " + ex.ToString());
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

        private void RegisterUriWithServer(Uri newChannelUri)
        {
            //Hardcode for solution - need to be updated in case the REST WCF service address change
            string baseUri = "http://localhost/RegistrationService/Register?uri={0}";
            string theUri = String.Format(baseUri, newChannelUri.ToString());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                    Dispatcher.BeginInvoke(() => {
                        txtURI.Text = "changing uri to " + newChannelUri.ToString();
                    });
                else
                    Dispatcher.BeginInvoke(() =>
                    {
                        txtURI.Text = "registration failed " + e.Error.Message;
                    });
            };
            client.DownloadStringAsync(new Uri(theUri));

        }

        private void UnregisterUriFromServer(Uri oldChannelUri)
        {
            //Hardcode for solution - need to be updated in case the REST WCF service address change
            string baseUri = "http://localhost/RegistrationService/Unregister?uri={0}";
            string theUri = String.Format(baseUri, oldChannelUri.ToString());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                    Dispatcher.BeginInvoke(() =>
                    {
                        txtURI.Text = "unregistered uri " + oldChannelUri.ToString();
                    });
                else
                    Dispatcher.BeginInvoke(() =>
                    {
                        txtURI.Text = "registration delete failed " + e.Error.Message;
                    });
            };
            client.DownloadStringAsync(new Uri(theUri));
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
                    txtURI.Text = "Raw Notification Message Received";
                });
            }
        }
    }
}
