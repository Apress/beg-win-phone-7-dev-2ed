using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.ServiceModel;
using System.Diagnostics;

namespace PNServer
{
    public partial class PushNotifications : Form
    {
        public PushNotifications()
        {
            InitializeComponent();
        }

        string TilePushXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<wp:Notification xmlns:wp=\"WPNotification\">" +
                        "<wp:Tile>" +
                              "<wp:Count>{0}</wp:Count>" +
                              "<wp:Title>{1}</wp:Title>" +
                        "</wp:Tile>" +
                    "</wp:Notification>";

        string ToastPushXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<wp:Notification xmlns:wp=\"WPNotification\">" +
                                "<wp:Toast>" +
                                    "<wp:Text1>{0}</wp:Text1>" +
                                    "<wp:Text2>{1}</wp:Text2>" +
                                "</wp:Toast>" +
                            "</wp:Notification>";

        private void btnSendNotification_Click(object sender, EventArgs e)
        {
            if (txtURL.Text == string.Empty)
            {
                MessageBox.Show("Please enter a url");
                return;
            }

            if (txtTitle.Text == string.Empty || txtText.Text == string.Empty)
            {
                MessageBox.Show("Please enter text and title to send");
                return;
            }
            sendPushNotificationToClient(txtURL.Text);
        }

        private void sendPushNotificationToClient(string url)
        {
            HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(url);

            sendNotificationRequest.Method = "POST";
            sendNotificationRequest.Headers = new WebHeaderCollection();
            sendNotificationRequest.ContentType = "text/xml";

            sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "toast");
            sendNotificationRequest.Headers.Add("X-NotificationClass", "2");
            string str = string.Format(ToastPushXML, txtTitle.Text, txtText.Text);

            //sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "token");
            //sendNotificationRequest.Headers.Add("X-NotificationClass", "1"); //- tiles
            //string str = string.Format(TilePushXML, txtTitle.Text, txtText.Text);

            byte[] strBytes = new UTF8Encoding().GetBytes(str);
            sendNotificationRequest.ContentLength = strBytes.Length;
            using (Stream requestStream = sendNotificationRequest.GetRequestStream())
            {
                requestStream.Write(strBytes, 0, strBytes.Length);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];           //(Received|Dropped|QueueFull|)
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];   //(Connected|InActive|Disconnected|TempDisconnected)
                lblStatus.Text = "Status: " + notificationStatus + " : " + deviceConnectionStatus;
            }
            catch (Exception ex)
            {
                //handle 404 and other exceptions that may occur
                lblStatus.Text = "Failed to connect, exception detail: "  + ex.Message;
            }
        }
        
        private void PushNotifications_Load(object sender, EventArgs e)
        {
          ServiceHost host;
          try
          {
            host = new ServiceHost(typeof(RegistrationService));
            host.Open();
          }
          catch (TimeoutException timeoutException)
          {
            MessageBox.Show(String.Format("The service operation timed out. {0}", timeoutException.Message));
          }
          catch (CommunicationException communicationException)
          {
            MessageBox.Show(String.Format("Could not start service host. {0}", communicationException.Message));
          }
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == string.Empty || txtText.Text == string.Empty)
            {
                MessageBox.Show("Please enter text and title to send");
                return;
            }

            List<Uri> allSubscribersUri = RegistrationService.GetSubscribers();
            
            foreach (Uri subscriberUri in allSubscribersUri)
            {
                sendPushNotificationToClient(subscriberUri.ToString());
            }
        }
    }
}
