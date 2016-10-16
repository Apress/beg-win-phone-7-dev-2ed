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
using Microsoft.Phone.Info;
using Microsoft.Devices.Radio;

namespace ErrorHandlingDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        CalculatorServiceProxy.Service1Client _svc;
        public MainPage()
        {
            InitializeComponent();

            txtDeviceManufacturer.Text = DeviceExtendedProperties.GetValue("DeviceManufacturer").ToString();
            txtDeviceName.Text = DeviceExtendedProperties.GetValue("DName").ToString();

            _svc = new CalculatorServiceProxy.Service1Client();
            _svc.AddCompleted += (s, e) =>
            {
                txtAnswer.Text = e.Result.ToString();
            };
        }

        private void btnCallCalcService_Click(object sender, RoutedEventArgs e)
        {
            _svc.AddAsync(txtX.Text, txtY.Text);
        }
    }
}