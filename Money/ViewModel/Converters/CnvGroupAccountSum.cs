using Money.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    public class CnvGroupAccountSum : IValueConverter
    {
        public object Convert(object value, System.Type targetType,
                      object parameter,
                      System.Globalization.CultureInfo culture)
        {
            if (null == value)  return "";

            double sum = 0;
            
            foreach (object i in (ReadOnlyObservableCollection<object>)value)
                sum += ((AccSubTotal)i).Amount;

            return sum.ToString("0.0#");
        }

        public object ConvertBack(object value, System.Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
