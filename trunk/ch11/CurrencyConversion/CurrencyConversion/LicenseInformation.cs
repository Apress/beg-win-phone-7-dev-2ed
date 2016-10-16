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

namespace CurrencyConversion
{
    public sealed class LicenseInformation
    {
        // Fields
        private bool m_fIsTrial = true;

        // Methods
        public bool IsTrial()
        {
            int num = 0;
            if (num != 0)
            {
                this.m_fIsTrial = true;
            }
            return this.m_fIsTrial;
        }

        // Nested Types
        internal static class NativeMethods
        {
            // Fields
            internal const int S_FALSE = 1;
            internal const int S_OK = 0;
        }
    }
}
