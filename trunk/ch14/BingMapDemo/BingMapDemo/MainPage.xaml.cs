using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;

namespace BingMapDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _geoCoordinateWatcher; 

        public MainPage()
        {
            InitializeComponent();
            // Add your own BingMap Key
            bingMap.CredentialsProvider = new ApplicationIdCredentialsProvider("AlWWYPw-HBVuA3AvU5AKn0cbGNp7jqjX0Vk5KFCIUHGgJRSktD0PRomkCnDPntPB");
            
            // Remove Bing Map logo and copyrights in order to gain
            // extra space at the bottom of the map
            bingMap.LogoVisibility = Visibility.Collapsed;
            bingMap.CopyrightVisibility = Visibility.Collapsed;

            // Delcare GeoCoordinateWatcher with high accuracy
            // in order to use the device's GPS
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _geoCoordinateWatcher.MovementThreshold = 100;

            // Subscribe to the device's status changed event
            _geoCoordinateWatcher.StatusChanged += 
                new EventHandler<GeoPositionStatusChangedEventArgs>(_geoCoordinateWatcher_StatusChanged);

            // Subscribe to the device's position changed event
            // to receive GPS coordinates (longitude and latitude)
            _geoCoordinateWatcher.PositionChanged += 
                new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_geoCoordinateWatcher_PositionChanged);
        }

        private void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => ChangePosition(e)); 
        }

        private void ChangePosition(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            SetLocation(e.Position.Location.Latitude, e.Position.Location.Longitude, 10, true);
        }

        private void _geoCoordinateWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => StatusChanged(e)); 
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // check to make sure the location service starts successfully
            if (!_geoCoordinateWatcher.TryStart(true, TimeSpan.FromSeconds(5)))
            {
                MessageBox.Show("Please enable Location Service on the Phone.", "Warning", MessageBoxButton.OK);
            }
        }

        private void StatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    txtStatus.Text = "Location Service is disabled!";
                    break;
                case GeoPositionStatus.Initializing:
                    txtStatus.Text = "Initializing Location Service...";
                    break;
                case GeoPositionStatus.NoData:
                    txtStatus.Text = "Your position could not be located.";
                    SetLocation(0, 0, 10, false);
                    break;
                case GeoPositionStatus.Ready:
                    break;
            }
        }

        private void SetLocation(double latitude, double longitude, double zoomLevel, bool showLocator)
        {
            Location location = new Location();
            location.Latitude = latitude;
            location.Longitude = longitude;
            bingMap.SetView(location, zoomLevel);
            bingMapLocator.Location = location;
            if (showLocator)
            {
                locator.Visibility = Visibility.Visible;
                BlinkLocator.Begin();
            }
            else
            { 
                locator.Visibility = Visibility.Collapsed;
                BlinkLocator.Stop();
            }
        }
    }
}
