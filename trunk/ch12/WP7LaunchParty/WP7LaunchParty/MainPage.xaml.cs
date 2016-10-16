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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;

namespace WP7LaunchParty
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ShowEventDetails();
        }

        private void ShowEventDetails()
        {
            textBlockListTitle.Text = "WP7 Launch";
            ResourceManager rm = new ResourceManager("WP7LaunchParty.AppResources", Assembly.GetExecutingAssembly());
            textBlockListTitle.Text = rm.GetString("EventTitle");
            txtEventCostCaption.Text = rm.GetString("EventCost");
            txtEventDateCaption.Text = rm.GetString("EventDate");
            txtEventTimeCaption.Text = rm.GetString("EventTime");

            //create the date of November 6, 2010 at 9:00 PM
            DateTime dtLaunchDate = new DateTime(2010, 11, 6, 21, 0, 0);
            //make the cost equal to $5
            decimal decEventCost = 5.0M;

            //we can also take advantage of the ToString() method to return value in a specific culture
            // in our case, we will simply pass in the Current Thread culture
            txtEventDate.Text = dtLaunchDate.ToString("D", Thread.CurrentThread.CurrentCulture);
            txtEventTime.Text = dtLaunchDate.ToString("T");

            txtEventCost.Text = decEventCost.ToString("C");
        }

        private void ToggleEventLocale()
        {
            //default to English-US culture
            String cul = "en-US";

            if (button1.Content.ToString() == "Español")
            {
                //change the culture to Spanish
                cul = "es-ES";
                button1.Content = "English";
            }
            else
            {
                cul = "en-US";
                button1.Content = "Español";
            }

            CultureInfo newCulture = new CultureInfo(cul);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            ShowEventDetails();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLocale();
        }

    }
}
