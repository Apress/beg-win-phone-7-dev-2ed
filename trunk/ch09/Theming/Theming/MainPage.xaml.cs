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

namespace Theming
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Ellipse e = new Ellipse();
            e.Width = 100.0;
            e.Height = 120.0;
            e.StrokeThickness = 2.0;

            e.HorizontalAlignment = HorizontalAlignment.Left;
            e.VerticalAlignment = VerticalAlignment.Top;

            Color backgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"];

            //Color backgroundColor = Color.FromArgb(255, 255, 255, 255);
            e.Fill = new SolidColorBrush(backgroundColor);
            e.Margin = new Thickness(10, 300, 10, 10);

            ContentPanel.Children.Add(e);

        }
    }
}