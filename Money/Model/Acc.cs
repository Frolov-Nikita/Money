using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Money.Model
{
    public class Acc : INotifyPropertyChanged, IComparable
    {
        public Acc()
        {
            this.Gps = new ObservableCollection<Gp>();
            this.TransOrigin = new ObservableCollection<Tran>();
            this.TransDest = new ObservableCollection<Tran>();
        }

        public Acc(Acc a)
        {
            id = a.id;
            name = a.name;
            description = a.description;
            type = a.type;

            this.Gps = new ObservableCollection<Gp>();
            this.TransOrigin = new ObservableCollection<Tran>();
            this.TransDest = new ObservableCollection<Tran>();
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

        private string name = "Новый счет";
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

        private Tp type = Tp.active;
        public Tp Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                NotifyPropertyChanged();
            }
        }
        
        #region Ссылки

        private ObservableCollection<Gp> gps;
        public virtual ObservableCollection<Gp> Gps
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

        private ObservableCollection<Tran> transOrigin;
        public virtual ObservableCollection<Tran> TransOrigin
        {
            get
            {
                return transOrigin;
            }

            set
            {
                transOrigin = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Tran> transDest;
        public virtual ObservableCollection<Tran> TransDest
        {
            get
            {
                return transDest;
            }

            set
            {
                transDest = value;
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
            return Name.CompareTo(((Acc)obj).Name);
        }
        
        #endregion
        
    }

    public class AccSubTotal
    {

        public Acc Acc { get; set; }
        public double Amount { get; set; }
    }


    /// <summary>
    /// Типы счетов
    /// </summary>
    public enum Tp : byte {
        [Description("Источник")]
        input,
        [Description("Актив")]
        active,
        [Description("Обязательства")]
        credit,
        [Description("Затраты")]
        output }


}
