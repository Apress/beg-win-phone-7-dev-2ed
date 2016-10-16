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
using Microsoft.Phone.Notification;

namespace UnsafeCode
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		unsafe public MainPage()
		{
			int i=5;
			InitializeComponent();
			SquarePtrParam(&i);
			PageTitle.Text = i.ToString();
		}

		// Unsafe method: takes pointer to int:
		unsafe static void SquarePtrParam(int* p)
		{
			*p *= *p;
		}
	}
}
