using Microsoft.Phone.Controls;
using System.Threading;
using Microsoft.Phone.Reactive;
using System.Device.Location;
using System.Collections.Generic;
using System;

namespace GeoCoordinateWatcherDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _geoCoordinateWatcher; 

        public MainPage()
        {
            InitializeComponent();

            // initialize GeoCoordinateWatcher
            _geoCoordinateWatcher = new GeoCoordinateWatcher();

            // PositionChanged event will receive GPS data
            _geoCoordinateWatcher.PositionChanged += 
                new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>
                    (_geoCoordinateWatcher_PositionChanged);

            // simulateGpsThread will start Reactive Extension
            // where EmulatePositionChangedEvents will be feeding 
            // the data to PositionChanged event
            Thread simulateGpsThread = new Thread(SimulateGPS);
            simulateGpsThread.Start();
        }

        // Reactive Extension that intercepts the _geoCoordinateWatcher_PositionChanged
        // in order to feed the GPS data.
        private void SimulateGPS()
        {
            var position = GPSPositionChangedEvents().ToObservable();
            position.Subscribe(evt => _geoCoordinateWatcher_PositionChanged(null, evt));
        }

        private static IEnumerable<GeoPositionChangedEventArgs<GeoCoordinate>> GPSPositionChangedEvents()
        {
            Random random = new Random();

            // feed the GPS data
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));

                // randomly generate GPS data, latitude and longitude.
                double latitude = (random.NextDouble() * 180.0) - 90.0; // latitude is between -90 and 90
                double longitude = (random.NextDouble() * 360.0) - 180.0; // longitude is between -180 and 180

                yield return new GeoPositionChangedEventArgs<GeoCoordinate>(
                  new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, new GeoCoordinate(latitude, longitude)));
            }
        }


        private void _geoCoordinateWatcher_PositionChanged(object sender
            , GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                txtLatitude.Text = e.Position.Location.Latitude.ToString();
                txtLongitude.Text = e.Position.Location.Longitude.ToString();
            });
        }
    }
}