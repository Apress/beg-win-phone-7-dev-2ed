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

namespace CurrencyConversion
{
    public partial class MoreOptions : PhoneApplicationPage
    {
        Double dblExchgRate;
        Decimal decTotalToConvert;

        public MoreOptions()
        {
            InitializeComponent();
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string strExchgRate = "";
            string strTotalToConvert = "";

            if (NavigationContext.QueryString.TryGetValue("rate", out strExchgRate))
                dblExchgRate = Convert.ToDouble(strExchgRate);

            if (NavigationContext.QueryString.TryGetValue("total", out strTotalToConvert))
                decTotalToConvert = Convert.ToDecimal(strTotalToConvert);

        }

        private void btnCalculateDamage_Click(object sender, RoutedEventArgs e)
        {
            decimal decTotalToReceive;
            decimal decTotalAccordingToConversionRate;

            decTotalToReceive = Convert.ToDecimal(txtExchangeRateQuoted.Text) * decTotalToConvert;
            decTotalAccordingToConversionRate = Convert.ToDecimal(dblExchgRate) * decTotalToConvert;

            txtDamageExplained.Text = "With exchange rate quoted, you will receive " + decTotalToReceive.ToString() + "\r\n";
            txtDamageExplained.Text = txtDamageExplained.Text +  "Given market exchange rate, you should receive " + decTotalAccordingToConversionRate.ToString() + "\r\n";

            txtDamageExplained.Text = txtDamageExplained.Text + "You lose " + (decTotalAccordingToConversionRate - decTotalToReceive).ToString();
        }
    }
}