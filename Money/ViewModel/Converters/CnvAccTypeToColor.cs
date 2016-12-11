using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Money.ViewModel.Converters
{
    [ValueConversion(typeof(long?), typeof(System.Windows.Media.Brush))]
    public class CnvAccTypeToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            System.Windows.Media.BrushConverter converter = new System.Windows.Media.BrushConverter();
            if(value==null)return converter.ConvertFromString("#FFFfFffF");

            int V = 0;
            if (value.GetType() == typeof(int)) V = (int)value;
            if (value.GetType() == typeof(uint)) V = (int)((uint)value);
            if (value.GetType() == typeof(long)) V = (int)((long)value);

            switch (V) 
            {
                case 1: //Доход
                    return converter.ConvertFromString("#FF1B0773");
                case 2: //Актив
                    return converter.ConvertFromString("#FF008500");
                case 3: //Обяз
                    return converter.ConvertFromString("#FFA68600"); 
                case 4: //Расход
                    return converter.ConvertFromString("#FFA60000");
                default:
                    return System.Windows.Media.Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("нельзя цвет обратно преобразовать.");
        }
    }


}
