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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Money.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Money.ViewModel.WindowWBViewModel VM = null)
        {
            //DataContext = VM??new ViewModel.MainWindowViewModel();
            InitializeComponent();
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccGp.ItemsSource);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");            
            //view.GroupDescriptions.Add(groupDescription);
        }
    }
}
