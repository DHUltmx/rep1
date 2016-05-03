using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ElevatorSystem.UI.Converters
{
    /// <summary>
    /// Converts the interer collection to custom strings
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    [ValueConversion(typeof(IList<int>), typeof(IList<string>))]
    public class FloorSelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<int> integerList = (IList<int>)value;
            IList<string> stringList = new List<string>();

            foreach(int num in integerList)
            {
                string numAsString = num == 0 ? "Ground" : num.ToString();
                stringList.Add(numAsString);
            }

            return stringList;         
        }

        public object ConvertBack(object value, Type targetType, object paramter, CultureInfo culture)
        {
            IList<string> stringList = (IList<string>)value;
            IList<int> integerList = new List<int>();

            foreach(string str in stringList)
            {
                int floorAsInt = str == "Ground" ? 0 : System.Convert.ToInt32(str);
                integerList.Add(floorAsInt);
            }

            return integerList;
        }
    }
}
