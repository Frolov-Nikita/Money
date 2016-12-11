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
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccGp.ItemsSource);
            //view.GroupDescriptions.Add(new PropertyGroupDescription("Acc.Gps"));
        }
    }
}
