using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Money.Model
{
    public class Acc : INotifyPropertyChanged
    {
        public Acc()
        {
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

        private string name = "unnamed";
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
        #endregion

        //public void RecalcAmount()
        //{
        //    amount = (from tD in TransDest.AsQueryable()
        //              group tD by tD.AccDest into gD
        //              select gD.Sum(t => t.Amount)).FirstOrDefault() -

        //           (from tO in TransOrigin.AsQueryable()
        //            group tO by tO.AccOrigin into gO
        //            select gO.Sum(t => t.Amount)).FirstOrDefault();
        //}
    }

    public class AccSubTotal
    {
        public Acc Acc { get; set; }
        public double Amount { get; set; }
    }


    /// <summary>
    /// Типы счетов
    /// </summary>
    public enum Tp : byte { input, active, credit, output }


}
