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

namespace RxSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            IObservable<int> source = Observable.Range(5, 3);

            IDisposable subscription = source.Subscribe(x => 
                textBlock1.Text += String.Format(" OnNext: {0}", x), 
                ex => textBlock1.Text += String.Format(" OnError: {0}", ex.Message), 
                () => textBlock1.Text += " OnCompleted");

            subscription.Dispose();
        }
    }
}
