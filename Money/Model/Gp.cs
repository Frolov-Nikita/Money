using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money.Model
{
    public class Gp : INotifyPropertyChanged, IComparable
    {
        public Gp()
        {
            this.Accounts = new ObservableCollection<Acc>();
        }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        #region Ссылки

        private ObservableCollection<Acc> accounts;
        public virtual ObservableCollection<Acc> Accounts
        {
            get
            {
                return accounts;
            }

            set
            {
                accounts = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(object obj)
        {
            return Name.CompareTo(((Gp)obj).Name);
        }
        #endregion
    }
}
