using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    [ValueConversion(typeof(double?), typeof(System.Windows.Media.Brush))]
    public class CnvAmountToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(double)) return null;
            if ((double)value == 0)
                return System.Windows.Media.Brushes.Black;
            if ((double)value > 0)
                return System.Windows.Media.Brushes.DarkGreen;
            else
                return System.Windows.Media.Brushes.DarkRed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("нельзя цвет обратно преобразовать.");
        }
    }
}
