using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    [ValueConversion(typeof(bool), typeof(System.Windows.GridLength))]
    public class CnvBoolToGrigLength : IValueConverter
    {
        System.Windows.GridLength GL = new System.Windows.GridLength((double)0);

        public object Convert(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
            {
                if (GL.Value < 50) GL = new System.Windows.GridLength(100.0);
            }
            else
            {
                GL = new System.Windows.GridLength(0.0);
            }           

            return GL;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            GL = (System.Windows.GridLength)value;
            if (GL.Value > 0)
                return true;
            return false;
        }
    }
}
