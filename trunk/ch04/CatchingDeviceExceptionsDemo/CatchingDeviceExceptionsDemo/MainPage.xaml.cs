using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;

namespace CatchingDeviceExceptionsDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer _acc;

        public MainPage()
        {
            InitializeComponent();
            _acc = new Accelerometer();
        }

        private void btnStartAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _acc.Start();

                _acc.ReadingChanged += (s1, e1) =>
                    {
                        // Do something with captured accelerometer data
                    };
            }
            catch (AccelerometerFailedException ex)
            {
                string errorMessage = string.Format(@"
                            Accelerometer threw an error with ErrorId {0} during the start operation
                            with error message {1}                            
                            ", ex.ErrorId, ex.Message);
                MessageBox.Show(errorMessage);
            }
        }

        private void btnStopAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 _acc.Stop();
            }
            catch (AccelerometerFailedException ex)
            {
                string errorMessage = string.Format(@"
                            Accelerometer threw an error with ErrorId {0} during the stop operation
                            with error message {1}                            
                            ", ex.ErrorId, ex.Message);
                MessageBox.Show(errorMessage);
            }
        }
    }
}