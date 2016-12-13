using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Money.View
{
    /// <summary>
    /// Interaction logic for WindowWB.xaml
    /// </summary>
    public partial class WindowWB : Window
    {
        public WindowWB()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccGp.ItemsSource);

            PropertyGroupDescription pgd = new PropertyGroupDescription(null, new CnvGroupingAccSubTotal()) ;
            //pgd.GroupNames.Add(new {Name = "Не сгруппированные" });
            view.GroupDescriptions.Add(pgd);
        }

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
}
