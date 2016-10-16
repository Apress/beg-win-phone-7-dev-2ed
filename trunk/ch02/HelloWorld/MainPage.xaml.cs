using System.Windows;
using Microsoft.Phone.Controls;

namespace HelloWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "Hello World!";
        }
    }
}