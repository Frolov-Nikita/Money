using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Money
{
    using Model;
    using ViewModel;
    using View;
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //BookContext BC = new BookContext((new DB()).MySqlConn);
            //WBViewModel WBViewModel = new WBViewModel(BC);
            //View.WB WB = new View.WB() { DataContext = WBViewModel };
            //WB.Show();
        }
    }
}
