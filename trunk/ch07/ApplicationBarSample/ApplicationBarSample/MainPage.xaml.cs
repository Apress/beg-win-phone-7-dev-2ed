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
using Microsoft.Phone.Shell;

namespace ApplicationBarSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            //ApplicationBar = new ApplicationBar();
            //ApplicationBar.IsVisible = true;
            //ApplicationBar.IsMenuEnabled = true;

            //ApplicationBarIconButton btnAdd = new ApplicationBarIconButton(new Uri("/Images/appbar.add.rest.png", UriKind.Relative));
            //btnAdd.Text = "add";
            //ApplicationBarIconButton btnSave = new ApplicationBarIconButton(new Uri("/Images/appbar.save.rest.png", UriKind.Relative));
            //btnSave.Text = "save";
            //ApplicationBarIconButton btnDelete = new ApplicationBarIconButton(new Uri("/Images/appbar.delete.rest.png", UriKind.Relative));
            //btnDelete.Text = "delete";

            //ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem("Menu Item 1");
            //ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem("Menu Item 2");
            
            //ApplicationBar.Buttons.Add(btnAdd);
            //ApplicationBar.Buttons.Add(btnSave);
            //ApplicationBar.Buttons.Add(btnDelete);

            //ApplicationBar.MenuItems.Add(menuItem1);
            //ApplicationBar.MenuItems.Add(menuItem2);

            //btnAdd.Click += new EventHandler(btnAdd_Click);
            //btnSave.Click += new EventHandler(btnSave_Click);

            //menuItem1.Click += new EventHandler(menuItem1_Click);
        }

        void menuItem1_Click(object sender, EventArgs e)
        {
            textBlock1.Visibility = Visibility.Visible;
            textBlock1.Text = "You just clicked on a menu item";
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            textBlock1.Text = "Thank you, " + textBox1.Text;
            textBox1.Visibility = Visibility.Collapsed;
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            textBox1.Visibility = Visibility.Visible;
            textBlock1.Visibility = Visibility.Visible;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

        }
    }
}
