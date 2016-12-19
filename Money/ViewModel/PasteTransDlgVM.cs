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
using Money.ViewModel.Converters;

namespace Money.ViewModel
{
    public class PasteTransDlgVM : INotifyPropertyChanged
    {

        #region свойства
        private string text;
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                NotifyPropertyChanged();
            }
        }

        private BindingList<Acc> accs;
        public BindingList<Acc> Accs
        {
            get
            {
                return accs;
            }

            set
            {
                accs = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Tran> newTrans;
        public ObservableCollection<Tran> NewTrans
        {
            get
            {
                return newTrans;
            }

            set
            {
                newTrans = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region command
        public CommandRef OkCmd { get; set; }
        public CommandRef CancelCmd { get; set; }

        #endregion

        // конструктор
        public PasteTransDlgVM()
        {
            #region комманды
            
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

        }//public PasteTransDlgVM()

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
