using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace IsolatedStorageStoreImageDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string ImageFileName = null;

        WebClient _webClient; // Used for downloading the image first time from the web site

        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            
            _webClient = new WebClient();
            _webClient.OpenReadCompleted += (s1, e1) =>
                {
                    if (e1.Error == null)
                   {
                       try
                       {
                            bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);

                            if (isSpaceAvailable)
                            {
                                // Save Image file to Isolated Storage
                                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(ImageFileName, 
                                                FileMode.Create, 
                                                IsolatedStorageFile.GetUserStoreForApplication()))
                                {
                                    long imgLen = e1.Result.Length;
                                    byte[] b = new byte[imgLen];
                                    e1.Result.Read(b, 0, b.Length);
                                    isfs.Write(b, 0, b.Length);
                                    isfs.Flush();
                                }

                                LoadImageFromIsolatedStorage(ImageFileName);
                            }
                            else
                            {
                                BitmapImage bmpImg = new BitmapImage();
                                bmpImg.SetSource(e1.Result);
                                image1.Source = bmpImg;
                            }
                           
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                    }
                };
        }

        // Check to make sure there are enough space available on the phone
        // in order to save the image that we are downloading on to the phone
        private bool IsSpaceIsAvailable(long spaceReq)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
            
                long spaceAvail = store.AvailableFreeSpace;
                if (spaceReq > spaceAvail)
                {
                    return false;
                }
                return true;
            }
        }

        private void btnGetImage_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                bool fileExist = isf.FileExists(ImageFileName);
                
                if (fileExist)
                {
                    LoadImageFromIsolatedStorage(ImageFileName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtImageUrl.Text))
                    {
                        // Use Uri as image file name
                        Uri uri = new Uri(txtImageUrl.Text);
                        ImageFileName = uri.AbsolutePath.Substring(uri.AbsolutePath.LastIndexOf('/')+1);
                        _webClient.OpenReadAsync(new Uri(txtImageUrl.Text));
                    }
                }

            }
           
        }

        private void LoadImageFromIsolatedStorage(string imageFileName)
        {
            // Load Image from Isolated storage
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream = isf.OpenFile(imageFileName, FileMode.Open))
                {
                    BitmapImage bmpImg = new BitmapImage();
                    bmpImg.SetSource(isoStream);
                    image1.Source = bmpImg;
                }
            }
        }

    }
}