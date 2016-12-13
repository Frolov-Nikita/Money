using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    public class CnvIsGpInAccColor : IMultiValueConverter
    {
        public Model.Acc Acc;

        public object Convert(object[] value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //if (value[0].GetType() != typeof(Model.Gp)) return System.Windows.Media.Brushes.Red;
            if (((Model.Acc)value[1]).Gps.Contains((Model.Gp)value[0]))
                return System.Windows.Media.Brushes.Green;
            else
                return System.Windows.Media.Brushes.Blue;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //return new object[0];
            throw new NotImplementedException("нельзя цвет обратно преобразовать.");
        }
    }


}
