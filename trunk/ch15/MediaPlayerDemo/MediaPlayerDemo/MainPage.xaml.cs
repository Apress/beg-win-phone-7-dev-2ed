using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace MediaPlayerDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // _updatingMediaTimeline is absolutely necessary to
        // make sure we do not cause adverse effect by 
        // changing the time line value by
        // moving forwards or backwards while the
        // time line is being changed.
        private bool _updatingMediaTimeline;

        public MainPage()
        {
            InitializeComponent();

            _updatingMediaTimeline = false;

            
            mediaPlayer.Source = new Uri(txtUrl.Text);

            // rewinds the media player to the beginning
            mediaPlayer.Position = System.TimeSpan.FromSeconds(0);

            // Download indicator
            mediaPlayer.DownloadProgressChanged += (s, e) =>
                {
                    lblDownload.Text = string.Format("Downloading {0:0.0%}", mediaPlayer.DownloadProgress);
                };

            // Handle media buffering
            mediaPlayer.BufferingTime = TimeSpan.FromSeconds(Convert.ToDouble(txtBufferingTime.Text));
            mediaPlayer.BufferingProgressChanged += (s, e) =>
                {
                    lblBuffering.Text = string.Format("Buffering {0:0.0%}", mediaPlayer.BufferingProgress);
                };

            // Updates the media time line (slider control) with total time played
            // and updates the status with the time played
            CompositionTarget.Rendering += (s, e) =>
                {
                    _updatingMediaTimeline = true;
                    TimeSpan duration = mediaPlayer.NaturalDuration.TimeSpan;
                    if (duration.TotalSeconds != 0)
                    {
                        double percentComplete = mediaPlayer.Position.TotalSeconds / duration.TotalSeconds;
                        mediaTimeline.Value = percentComplete;
                        TimeSpan mediaTime = mediaPlayer.Position;
                        string text = string.Format("{0:00}:{1:00}", 
                            (mediaTime.Hours * 60) + mediaTime.Minutes, mediaTime.Seconds);

                        if (lblStatus.Text != text)
                            lblStatus.Text = text;

                        _updatingMediaTimeline = false;
                    }
                };
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.CanPause)
            {
                mediaPlayer.Pause();
                lblStatus.Text = "Paused";
            }
            else
            {
                lblStatus.Text = "Can not be Paused. Please try again!";
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Position = System.TimeSpan.FromSeconds(0);
            lblStatus.Text = "Stopped";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (this.CanPlay())
            {
                mediaPlayer.Play();
            }
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if (lblSoundStatus.Text.Equals("Sound On", StringComparison.CurrentCultureIgnoreCase))
            {
                lblSoundStatus.Text = "Sound Off";
                mediaPlayer.IsMuted = true;
            }
            else
            {
                lblSoundStatus.Text = "Sound On";
                mediaPlayer.IsMuted = false;
            }
            
        }

        private void mediaTimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_updatingMediaTimeline && mediaPlayer.CanSeek)
            {
                TimeSpan duration = mediaPlayer.NaturalDuration.TimeSpan;
                int newPosition = (int)(duration.TotalSeconds * mediaTimeline.Value);
                mediaPlayer.Position = new TimeSpan(0, 0, newPosition);
            }
        }

        private void btnMediaPlayerLauncher_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerLauncher player = new MediaPlayerLauncher();
            player.Media = new Uri(txtUrl.Text);
            player.Show();
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
                if (MessageBox.Show("Is it ok to stop currently playing music and play our video?", "Can play our video?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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


//player.Media = new Uri("ARCastMDISilverlightGridComputing_ch9.wmv", UriKind.Relative);
//player.Location = MediaLocationType.Install;
