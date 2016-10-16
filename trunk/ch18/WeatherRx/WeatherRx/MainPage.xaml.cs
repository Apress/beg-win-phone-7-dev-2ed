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
using Microsoft.Phone.Reactive;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using WeatherRx.svcWeather;

namespace WeatherRx
{
    public partial class MainPage : PhoneApplicationPage
    {
        svcWeather.WeatherForecastSoapClient weatherClient = new svcWeather.WeatherForecastSoapClient();
        IObservable<IEvent<GetWeatherByZipCodeCompletedEventArgs>> _weather;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //WireUpWeatherEvents();
            WireUpKeyEvents();
        }

        private void WireUpWeatherEvents()
        {
            GetWeatherSubject();
            _weather.ObserveOn(Deployment.Current.Dispatcher)
                .Timeout(TimeSpan.FromSeconds(5))
                .Subscribe(evt =>
                {
                    if (evt.EventArgs.Result.Details != null)
                    {
                        lblWeatherFahrenheit.Text = "Current Weather, Fahrenheit: " + evt.EventArgs.Result.Details[0].MinTemperatureF.ToString() + " - " + evt.EventArgs.Result.Details[0].MaxTemperatureF.ToString();
                        lblCelsius.Text = "Current Weather, Celsius: " + evt.EventArgs.Result.Details[0].MinTemperatureC.ToString() + " - " + evt.EventArgs.Result.Details[0].MaxTemperatureC.ToString();
                        if(evt.EventArgs.Result.Details[0].WeatherImage!=null)
                            imgWeather.Source = new BitmapImage(new Uri(evt.EventArgs.Result.Details[0].WeatherImage, UriKind.Absolute));
                    }
                },
                ex =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => lblStatus.Text = ex.Message);
                    Deployment.Current.Dispatcher.BeginInvoke(() => btnQuit.Visibility=System.Windows.Visibility.Visible);
                    Deployment.Current.Dispatcher.BeginInvoke(() => btnRetry.Visibility = System.Windows.Visibility.Visible);
                }
            );
        }

        private void GetWeatherSubject()
        {
            if (_weather == null)
            {
                _weather = Observable.FromEvent<svcWeather.GetWeatherByZipCodeCompletedEventArgs>(weatherClient, "GetWeatherByZipCodeCompleted");
            }
        }

        private void WireUpKeyEvents()
        {
            var keys = Observable.FromEvent<KeyEventArgs>(txtZipCode, "KeyUp").Throttle(TimeSpan.FromSeconds(1)).DistinctUntilChanged();
            keys.ObserveOn(Deployment.Current.Dispatcher).Subscribe(evt =>
            {
                if (txtZipCode.Text.Length >= 5)
                {
                    WireUpWeatherEvents();
                    weatherClient.GetWeatherByZipCodeAsync(txtZipCode.Text);
                }
            });

        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            btnQuit.Visibility = System.Windows.Visibility.Collapsed;
            btnRetry.Visibility = System.Windows.Visibility.Collapsed;
            lblStatus.Text = "";

            WireUpWeatherEvents();
            weatherClient.GetWeatherByZipCodeAsync(txtZipCode.Text);
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            btnQuit.Visibility = System.Windows.Visibility.Collapsed;
            btnRetry.Visibility = System.Windows.Visibility.Collapsed;
            lblStatus.Text = "";

        }
    }
}
