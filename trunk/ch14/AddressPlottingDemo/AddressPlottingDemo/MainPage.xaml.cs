using System.Windows;
using Microsoft.Phone.Controls;
using AddressPlottingDemo.BingMapGeoCodeService;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using System.Linq;

namespace AddressPlottingDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeocodeServiceClient _svc;
        public MainPage()
        {
            InitializeComponent();

            // instantiate Bing Map GeocodeService
            _svc = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            _svc.GeocodeCompleted += (s, e) =>
                 {
                     // sort the returned record by ascending confidence in order for
                     // highest confidence to be on the top. Based on the numeration High value is
                     // at 0, Medium value at 1 and Low volue at 2
                     var geoResult = (from r in e.Result.Results 
                                      orderby (int)r.Confidence ascending 
                                      select r).FirstOrDefault();
                     if (geoResult != null)
                     {
                         this.SetLocation(geoResult.Locations[0].Latitude,
                             geoResult.Locations[0].Longitude,
                             10,
                             true);
                     }
                 };
        }

        private void SetLocation(double latitude, double longitude, double zoomLevel, bool showLocator)
        {
            // Move the pushpin to geo coordinate
            Microsoft.Phone.Controls.Maps.Platform.Location location = new Microsoft.Phone.Controls.Maps.Platform.Location();
            location.Latitude = latitude;
            location.Longitude = longitude;
            bingMap.SetView(location, zoomLevel);
            bingMapLocator.Location = location;
            if (showLocator)
            {
                locator.Visibility = Visibility.Visible;
            }
            else
            { 
                locator.Visibility = Visibility.Collapsed;
            }
        }

        private void btnPlot_Click(object sender, RoutedEventArgs e)
        {
            BingMapGeoCodeService.GeocodeRequest request = new BingMapGeoCodeService.GeocodeRequest();

            // Only accept results with high confidence.
            request.Options = new GeocodeOptions()
            {
                Filters = new ObservableCollection<FilterBase>
                {
                    new ConfidenceFilter()
                    {
                        MinimumConfidence = Confidence.High
                    }
                }
            };


            request.Credentials = new Credentials()
            {
                ApplicationId = "AlWWYPw-HBVuA3AvU5AKn0cbGNp7jqjX0Vk5KFCIUHGgJRSktD0PRomkCnDPntPB"
            };

            request.Query = txtAddress.Text;
            
            // Make asynchronous call to fetch the geo coordinate data.
            _svc.GeocodeAsync(request);
        }
    }
}