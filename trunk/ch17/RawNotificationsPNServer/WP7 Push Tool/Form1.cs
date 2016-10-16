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

namespace WP7_Push_Notifications
{
    public partial class PushNotifications : Form
    {
        public PushNotifications()
        {
            InitializeComponent();
        }

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

            HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(txtURL.Text);

            sendNotificationRequest.Method = "POST";
            sendNotificationRequest.Headers = new WebHeaderCollection();
            sendNotificationRequest.ContentType = "text/xml";

            sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "");
            sendNotificationRequest.Headers.Add("X-NotificationClass", "3"); //- raw, deliver immediately

            string str = string.Format(txtTitle.Text + "\r\n" + txtText.Text);
            byte[] strBytes = new UTF8Encoding().GetBytes(str);
            sendNotificationRequest.ContentLength = strBytes.Length;
            using (Stream requestStream = sendNotificationRequest.GetRequestStream())
            {
                requestStream.Write(strBytes, 0, strBytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
            string notificationStatus = response.Headers["X-NotificationStatus"];           //(Received|Dropped|QueueFull|)
            string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];   //(Connected|InActive|Disconnected|TempDisconnected)
            lblStatus.Text = "Status: " + notificationStatus + " : " + deviceConnectionStatus;
        }
    }
}
