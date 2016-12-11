using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{

    [ValueConversion(typeof(bool?), typeof(System.Windows.Visibility))]
    public class CnvBoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            switch ((bool?)value)
            {
                case true:
                    return System.Windows.Visibility.Visible;
                default:
                    return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("нельзя обратно преобразовывать.");
        }
    }


}
