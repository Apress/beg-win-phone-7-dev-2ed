using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;

namespace MoveBallDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Accelerometer _ac;

        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            ball.SetValue(Canvas.LeftProperty, ContentGrid.Width / 2);
            ball.SetValue(Canvas.TopProperty, ContentGrid.Height / 2);

            _ac = new Accelerometer();
            _ac.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(ac_ReadingChanged);
        }

        private void ac_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyReadingChanged(e));
        }

        private void MyReadingChanged(AccelerometerReadingEventArgs e)
        {
            double distanceToTravel = 2;
            double accelerationFactor = Math.Abs(e.Z) == 0 ? 0.1 : Math.Abs(e.Z);
            double ballX = (double)ball.GetValue(Canvas.LeftProperty) + distanceToTravel * e.X / accelerationFactor;
            double ballY = (double)ball.GetValue(Canvas.TopProperty) - distanceToTravel * e.Y / accelerationFactor;

            if (ballX < 0)
            {
                ballX = 0;
            }
            else if (ballX > ContentGrid.Width)
            {
                ballX = ContentGrid.Width;
            }

            if (ballY < 0)
            {
                ballY = 0;
            }
            else if (ballY > ContentGrid.Height)
            {
                ballY = ContentGrid.Height;
            }

            ball.SetValue(Canvas.LeftProperty, ballX);
            ball.SetValue(Canvas.TopProperty, ballY);
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_ac == null)
            {
                _ac = new Accelerometer();
            }
            _ac.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_ac == null)
            {
                _ac = new Accelerometer();
            }
            _ac.Stop();
        }
    }
}
