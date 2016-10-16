using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;

namespace PhotoCapture
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CameraCaptureTask cameraCaptureTask;
        WriteableBitmap original;
        byte[] imageBits;

        WriteableBitmap resized;
        private PhotoChooserTask photoChooserTask;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += PhotoChooserTaskCompleted;

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(PhotoChooserTaskCompleted);

        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            cameraCaptureTask.Show();
        }

        private void btnOpenPhoto_Click(object sender, EventArgs e)
        {
            photoChooserTask.Show();
        }

        private void PhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto != null)
            {
                imageBits = new byte[(int)e.ChosenPhoto.Length];
                e.ChosenPhoto.Read(imageBits, 0, imageBits.Length);
                e.ChosenPhoto.Seek(0, System.IO.SeekOrigin.Begin);

                var bitmapImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                this.imgPhoto.Source = bitmapImage;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var library = new MediaLibrary();
                library.SavePicture("PhotoCapture Photo", imageBits);

                txtStatus.Text = "Successfully saved photo.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Failed to save photo. Exception: " + ex.Message;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;
                if (queryStrings.ContainsKey("token"))
                {
                    MediaLibrary library = new MediaLibrary();
                    Picture picture = library.GetPictureFromToken(queryStrings["token"]);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(picture.GetImage());
                    WriteableBitmap picLibraryImage = new WriteableBitmap(bitmap);
                    imgPhoto.Source = picLibraryImage;
                }

                if (queryStrings.ContainsKey("FileId"))
                {
                    MediaLibrary library = new MediaLibrary();
                    Picture picture = library.GetPictureFromToken(queryStrings["FileId"]);


                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(picture.GetImage());
                    WriteableBitmap picLibraryImage = new WriteableBitmap(bitmap);
                    imgPhoto.Source = picLibraryImage;
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() => txtStatus.Text = ex.Message);
            }
        }

        public void UploadPhoto()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://twitpic.com/api/upload");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
        }


        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                string encoding = "iso-8859-1";
                // End the operation
                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                string boundary = Guid.NewGuid().ToString();
                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

                string header = string.Format("--{0}", boundary);
                string footer = string.Format("--{0}--", boundary);

                StringBuilder contents = new StringBuilder();
                contents.AppendLine(header);

                string fileHeader = String.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"; ", "media", "testpic.jpg");
                string fileData = Encoding.GetEncoding(encoding).GetString(imageBits, 0, imageBits.Length);

                contents.AppendLine(fileHeader);
                contents.AppendLine(String.Format("Content-Type: {0};", "image/jpeg"));
                contents.AppendLine();
                contents.AppendLine(fileData);
                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "username"));
                contents.AppendLine();
                contents.AppendLine("BeginningWP7");

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "password"));
                contents.AppendLine();
                contents.AppendLine("windowsphone7");

                contents.AppendLine(footer);

                // Convert the string into a byte array.
                byte[] byteArray = Encoding.GetEncoding(encoding).GetBytes(contents.ToString());

                // Write to the request stream.
                postStream.Write(byteArray, 0, contents.ToString().Length);
                postStream.Close();

                // Start the asynchronous operation to get the response
                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() => txtStatus.Text = ex.Message);                
            }
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                // End the operation
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();

                XDocument doc = XDocument.Parse(responseString);
                XElement rsp = doc.Element("rsp");
                string status = rsp.Attribute(XName.Get("status")) != null ? rsp.Attribute(XName.Get("status")).Value : rsp.Attribute(XName.Get("stat")).Value;

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();

            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() => txtStatus.Text = ex.Message);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            UploadPhoto();
        }

    }
}