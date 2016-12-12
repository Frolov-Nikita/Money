using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.Entity;
using Money.Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;

namespace Money.ViewModel
{
    public class EditAccViewModel : INotifyPropertyChanged
    {

        #region свойства

        private Acc acc;
        public Acc Acc
        {
            get
            {
                return acc;
            }

            set
            {
                acc = value;
                NotifyPropertyChanged();
            }
        }

        private BindingList<Gp> gps;
        public BindingList<Gp> Gps
        {
            get
            {
                return gps;
            }

            set
            {
                gps = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region command
        public CommandRef AddGpCmd { get; set; }
        public CommandRef RemoveGpCmd { get; set; }

        public CommandRef OkCmd { get; set; }
        public CommandRef CancelCmd { get; set; }
        #endregion

        // конструктор
        public EditAccViewModel()
        {

            #region комманды
            AddGpCmd = new CommandRef()
            {
                ExecuteDelegate = a =>
                {
                    Gps.Add(new Gp() { Name = "Новая группа" });
                }
            };

            RemoveGpCmd = new CommandRef()
            {
                ExecuteDelegate = G =>
                {
                    if (((Gp)G).Accounts.Count > 0)
                        if (System.Windows.MessageBox.Show("Группа содержит счета. Все равно удалить?", "Подтвердите", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                            return;
                    Gps.Remove((Gp)G);
                }
            };

            OkCmd = new CommandRef()
            {
                ExecuteDelegate = win =>
                {
                    ((Window)win).DialogResult = true;
                    ((Window)win).Close();
                }
            };

            CancelCmd = new CommandRef()
            {
                ExecuteDelegate = win =>
                {
                    ((Window)win).DialogResult = false;
                    ((Window)win).Close();
                }
            };
            #endregion

        }//public MainWindowModelView()

        

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
