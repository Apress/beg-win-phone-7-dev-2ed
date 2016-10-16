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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Marketplace;

namespace TrialSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var lic = new LicenseInformation();

            if (lic.IsTrial() == true)
            {
                textBlock1.Text = "You are running a trial version of our software!";
                btnUpgrade.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock1.Text = "Thank you for using the full version of our software!";
            }
        }

        private void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}
