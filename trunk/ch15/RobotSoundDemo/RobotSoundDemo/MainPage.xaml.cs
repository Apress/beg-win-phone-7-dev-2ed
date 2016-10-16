using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace RobotSoundDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();       
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (CanPlay())
            {
                MoveRobot.Begin();

                robotSound.Stop();
                robotSound.Source = new System.Uri("sound26.wma", System.UriKind.Relative);

                System.Threading.Thread.Sleep(50);
                robotSound.Play();
            }
        }

        private bool CanPlay()
        {
            bool canPlay = false;

            FrameworkDispatcher.Update();
            if (MediaPlayer.GameHasControl)
            {
                canPlay = true;
            }
            else
            {
                if (MessageBox.Show("Is it ok to stop currently playing music?", "Can stop music?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    canPlay = true;
                    MediaPlayer.Pause();
                }
                else
                {
                    canPlay = false;
                }
            }

            return canPlay;
        }
    }
}
