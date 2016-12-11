using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money.Model
{
    public class Tran : INotifyPropertyChanged
    {

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

        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                NotifyPropertyChanged();
            }
        }

        private string description = "";
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

        private double amount = 0.0;
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                NotifyPropertyChanged();
            }
        }

        // внешние ключики в явном виде

        private int accDest_Id;
        public virtual int AccDest_Id
        {
            get
            {
                return accDest_Id;
            }

            set
            {
                accDest_Id = value;
                NotifyPropertyChanged();
            }
        }

        private int accOrigin_Id;
        public virtual int AccOrigin_Id
        {
            get
            {
                return accOrigin_Id;
            }

            set
            {
                accOrigin_Id = value;
                NotifyPropertyChanged();
            }
        }

        #region Навигационные ссылки
        
        private Acc accOrigin;
        public virtual Acc AccOrigin
        {
            get
            {
                return accOrigin;
            }

            set
            {
                accOrigin = value;
                NotifyPropertyChanged();
            }
        }

        private Acc accDest;
        public virtual Acc AccDest
        {
            get
            {
                return accDest;
            }

            set
            {
                accDest = value;
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
        #endregion
    }
}
