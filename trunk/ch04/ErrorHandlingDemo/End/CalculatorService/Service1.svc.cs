using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalculatorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        public int Add(object x, object y)
        {
            int xValue = CheckValue(x);
            int yValue = CheckValue(y);

            return xValue + yValue;
        }

        private int CheckValue(object value)
        {
            int convertedValue = -1;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (!int.TryParse(value.ToString(), out convertedValue))
            {
                throw new ArgumentException(string.Format("The value '{0}' is not an integer.", value));
            }

            return convertedValue;
        }
    }
}
