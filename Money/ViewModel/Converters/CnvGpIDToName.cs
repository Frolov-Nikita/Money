using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    public class CnvGpIDToName : IValueConverter
    {

        public DataTable   GPNames { get; set; }

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {

            var r = (from g in GPNames.AsEnumerable()
                    where g["ID"].ToString() == value.ToString()
                    select g["Name"].ToString()).ToArray();
            if (r.Length>0) return r[0];
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("нельзя цвет обратно преобразовать.");
        }
    }


}
