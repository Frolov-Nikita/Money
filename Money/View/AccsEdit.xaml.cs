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
    /// Interaction logic for AccsEdit.xaml
    /// </summary>
    public partial class AccsEdit : Window
    {
        public AccsEdit(ViewModel.AccsEditVieModel AEVM)
        {
            DataContext = AEVM;
            InitializeComponent();
            ((ViewModel.Converters.ConverterGpIDToName)Resources["ConverterGpIDToName"]).GPNames = AEVM.BookData.DS.Tables["AccGp"];
        }

    }
}
