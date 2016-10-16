using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;

namespace CaptureAccelerometerData
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer _ac;

        public MainPage()
        {
            InitializeComponent();
            
            _ac = new Accelerometer();
            _ac.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(_ac_ReadingChanged);
        }

        private void _ac_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => ProcessAccelerometerReading(e));
        }

        private void ProcessAccelerometerReading(AccelerometerReadingEventArgs e)
        {
            txtTime.Text = e.Timestamp.ToString();
            txtX.Text = e.X.ToString();
            txtY.Text = e.Y.ToString();
            txtZ.Text = e.Z.ToString();
            txtPitch.Text = RadianToDegree((Math.Atan(e.X / Math.Sqrt(Math.Pow(e.Y, 2) + Math.Pow(e.Z, 2))))).ToString();
            txtRoll.Text = RadianToDegree((Math.Atan(e.Y / Math.Sqrt(Math.Pow(e.X, 2) + Math.Pow(e.Z, 2))))).ToString();
            txtYaw.Text = RadianToDegree((Math.Atan(Math.Sqrt(Math.Pow(e.X, 2) + Math.Pow(e.Y, 2)) / e.Z))).ToString();
        }

        private double RadianToDegree(double radian)
        {
            return radian * (180.0 / Math.PI);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ac.Start();
            }
            catch (AccelerometerFailedException)
            {
                MessageBox.Show("Acceleromter failed to start.");
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ac.Stop();
            }
            catch (AccelerometerFailedException)
            {
                MessageBox.Show("Acceleromter failed to stop.");
            }
        }
    }
}
