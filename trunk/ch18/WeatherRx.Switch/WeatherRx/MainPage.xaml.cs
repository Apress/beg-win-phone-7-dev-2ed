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
        IObservable<IEvent<KeyEventArgs>> _keys;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            WireUpWeatherEvents();
            WireUpKeyEvents();
        }
        
        private void WireUpWeatherEvents()
        {
            GetKeys();
            var latestWeather = (from term in _keys
                       select GetWeatherSubject()
                          .Finally(() =>
                          {
                            Deployment.Current.Dispatcher.BeginInvoke(() => Debug.WriteLine("Disposed of prior subscription"));
                          })
             ).Switch(); 

            latestWeather.ObserveOnDispatcher()
                .Subscribe(evt =>
            {
                if (evt.EventArgs.Result != null)
                {
                    lblWeatherFahrenheit.Text = "Current Weather, Fahrenheit: " + evt.EventArgs.Result.Details[0].MinTemperatureF.ToString() + " - " + evt.EventArgs.Result.Details[0].MaxTemperatureF.ToString();
                    lblCelsius.Text = "Current Weather, Celsius: " + evt.EventArgs.Result.Details[0].MinTemperatureC.ToString() + " - " + evt.EventArgs.Result.Details[0].MaxTemperatureC.ToString();
                    if(evt.EventArgs.Result.Details[0].WeatherImage!=null)
                        imgWeather.Source = new BitmapImage(new Uri(evt.EventArgs.Result.Details[0].WeatherImage, UriKind.Absolute));
                }
            },
                ex => {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>lblStatus.Text = ex.Message);
                        }                
            );
        }

        private IObservable<IEvent<GetWeatherByZipCodeCompletedEventArgs>> GetWeatherSubject()
        {           
            return Observable.FromEvent<svcWeather.GetWeatherByZipCodeCompletedEventArgs>(weatherClient, "GetWeatherByZipCodeCompleted");                        
        }

        private void GetKeys()
        {
            if (_keys == null)
            {
                _keys = Observable.FromEvent<KeyEventArgs>(txtZipCode, "KeyUp").Throttle(TimeSpan.FromSeconds(1)).DistinctUntilChanged();
            }
        }

        private void WireUpKeyEvents()
        {
            GetKeys();
            _keys.ObserveOn(Deployment.Current.Dispatcher).Subscribe(evt =>
            {
                if (txtZipCode.Text.Length >= 5)
                {
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
    }
}
