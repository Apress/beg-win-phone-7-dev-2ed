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
using System.Windows.Resources;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;

namespace WindowsPhoneApplication1
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _updatingMediaTimeline;

        public MainPage()
        {
            InitializeComponent();

            CompositionTarget.Rendering += (s, e) =>
            {
                _updatingMediaTimeline = true;
                TimeSpan duration = mediaDuckSound.NaturalDuration.TimeSpan;
                if (duration.TotalSeconds != 0)
                {
                    double percentComplete = mediaDuckSound.Position.TotalSeconds / duration.TotalSeconds;
                    playTimeBar.Value = percentComplete;
                    TimeSpan mediaTime = mediaDuckSound.Position;
                    string text = string.Format("{0:00}:{1:00}", (mediaTime.Hours * 60) + mediaTime.Minutes, mediaTime.Seconds);

                    if (lblPlayTime.Text != text)
                        lblPlayTime.Text = text;

                    _updatingMediaTimeline = false;
                }
            };

            mediaDuckSound.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(mediaDuckSound_MediaFailed);
        }

        void mediaDuckSound_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnBasic_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("basic_quack.wav");
        }

        private void btnGreet_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("greeting_call.wav");
        }

        private void btnLand_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("comeback_call.wav");
        }

        private void btnHail_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("hail_call.wav");
        }

        private void btnLonesome_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("lonesome_hen.wav");
        }

        private void btnFeeding_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound("feeding_call.wav");
        }

        private void PlaySound(string soundFile)
        {
            mediaDuckSound.Stop();
            mediaDuckSound.Source = new Uri(soundFile, UriKind.Relative);

            mediaDuckSound.Position = System.TimeSpan.FromSeconds(0);
            mediaDuckSound.Volume = 1;
            System.Threading.Thread.Sleep(50); //TODO: remove this hack in release version have to do this in beta
            mediaDuckSound.Play();

        }

        //private void PlaySound(string soundFile)
        //{
        //    LoadIntoIsolatedStorate(soundFile);

        //    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream isoStream = isf.OpenFile(soundFile.Split('/')[1], FileMode.Open))
        //        {
        //            mediaDuckSound.Stop();
        //            mediaDuckSound.SetSource(isoStream);

        //            mediaDuckSound.Position = System.TimeSpan.FromSeconds(0);
        //            mediaDuckSound.Volume = 1;
        //            System.Threading.Thread.Sleep(50); //TODO: remove this hack in release version have to do this in beta
        //            mediaDuckSound.Play();
        //        }
        //    }
           
        //}

        private void playTimeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_updatingMediaTimeline)
            {
                TimeSpan duration = mediaDuckSound.NaturalDuration.TimeSpan;
                int newPosition = (int)(duration.TotalSeconds * playTimeBar.Value);
                mediaDuckSound.Position = new TimeSpan(0, 0, newPosition);
            }
        }


        //HACK:Just to make sound work by getting it from isolated storage
        //private void LoadIntoIsolatedStorate(string soundFile)
        //{
        //    StreamResourceInfo sri;
        //    sri = Application.GetResourceStream(new Uri(soundFile, UriKind.Relative));

        //    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        //if (!isf.FileExists(soundFile))
        //        //{
        //        using (IsolatedStorageFileStream soundFileStream = isf.CreateFile(soundFile.Split('/')[1]))
        //        {
        //            long soundLen = sri.Stream.Length;
        //            byte[] b = new byte[soundLen];
        //            sri.Stream.Read(b, 0, b.Length);
        //            soundFileStream.Write(b, 0, b.Length);
        //            soundFileStream.Flush();
        //        }

        //        //}
        //    }
        //}
    }
}
