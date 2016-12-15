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
    public class CnvGroupingAccSubTotal : IValueConverter
    {
        public object Convert(object value, System.Type targetType,
                      object parameter,
                      System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            Model.AccSubTotal A = (Model.AccSubTotal)value;
            if (A.Acc.Gps.Count > 0)
                return (from g in A.Acc.Gps select g.Name).ToList();

            return "Счета без группы";
        }

        public object ConvertBack(object value, System.Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }//public class CnvGroupingAccount : IValueConverter
}
