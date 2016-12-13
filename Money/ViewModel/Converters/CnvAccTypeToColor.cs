using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Money.Model;

namespace Money.ViewModel.Converters
{
    public class CnvAccTypeToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            System.Windows.Media.BrushConverter converter = new System.Windows.Media.BrushConverter();

            if (value==null)return converter.ConvertFromString("#FFFfFffF");
            
            switch ((Tp)value) 
            {
                case Tp.input: //Доход
                    return converter.ConvertFromString("#FF1B0773");
                case Tp.active: //Актив
                    return converter.ConvertFromString("#FF008500");
                case Tp.credit: //Обяз
                    return converter.ConvertFromString("#FFA68600"); 
                case Tp.output: //Расход
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
