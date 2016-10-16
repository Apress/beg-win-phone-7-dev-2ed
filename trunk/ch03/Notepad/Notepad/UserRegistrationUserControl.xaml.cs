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
using System.ComponentModel;
using Notepad.NotepadServiceProxy;

namespace Notepad
{
    public partial class UserRegistrationUserControl : UserControl
    {
        

        public UserRegistrationUserControl()
        {
            InitializeComponent();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            NotepadViewModel.Instance.AddUser(Guid.NewGuid(), txtUserName.Text);
        }
    }
}
