using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Tasks
{
    public class Data:INotifyPropertyChanged
    {
        private string _TextboxText = "";
        public string TextboxText
        {
            get { return _TextboxText; }
            set
            {
                _TextboxText = value;
                NotifyPropertyChanged("TextboxText");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
