using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Money.Model;

namespace Money.ViewModel
{
    public class ConnectionEditViewModel : INotifyPropertyChanged
    {        
        private DB db = new DB();

        public DB DB
        {
            get
            {
                return db;
            }

            set
            {
                db = value;
                NotifyPropertyChanged();
            }
        }


        public CommandRef OpenSQLiteDBDialog { get; set; }

        public ConnectionEditViewModel()
        {
            OpenSQLiteDBDialog = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog() { Multiselect = false };
                    if (ofd.ShowDialog() == true)
                    {
                        DB.SQLiteDataSource = ofd.FileName;
                    }
                }
            };
        }
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
