using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    [ValueConversion(typeof(bool?), typeof(System.Windows.Media.Brush))]
    public class CnvChangesToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            if ((bool)value == true)
                return System.Windows.Media.Brushes.Green;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
