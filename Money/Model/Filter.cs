using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Money.Model
{
    public class Filter : INotifyPropertyChanged
    {
        /*  ПРИМЕР Фильтра:
         *  Acc:Имя счета GP:Имя группы #хэштэг From:2012-06-01 00:00:00 To:01.10.2016
         *  (Acc OR GP ) AND hashtag AND date
         *  
         *  table.AsEnumerable()
         *      .ToDictionary<int, string>(row => row.Field<int>(col1),
                                row => row.Field<string>(col2));
         */

        Acc acc;
        public Acc Acc
        {
            get { return acc; }
            set
            {
                acc = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SQLString");
                NotifyPropertyChanged("ViewString");
            }
        }

        Gp gP;

        public Gp GP
        {
            get
            {
                return gP;
            }

            set
            {
                gP = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SQLString");
                NotifyPropertyChanged("ViewString");
            }
        }

        DateTime fromDate = DateTime.Now.AddDays(-61);

        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SQLString");
                NotifyPropertyChanged("ViewString");
            }
        }

        DateTime toDate = DateTime.Now.AddDays(+1);

        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SQLString");
                NotifyPropertyChanged("ViewString");
            }
        }

        string _HashTag = "";

        public string HashTag
        {
            get
            {
                return _HashTag;
            }

            set
            {
                _HashTag = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SQLString");
                NotifyPropertyChanged("ViewString");
            }
        }

        /// <summary>
        /// Собранная из полей инструкция фильтра коллекции транзакций
        /// </summary>
        public string SQLString
        {
            get
            {

                string f = "";
                if (Acc != null) f += " (AccOrigin.Name = '" + Acc.Name + "' OR AccDest.Name = '" + Acc.Name + "') ";

                if (FromDate != null)
                {
                    f += f != "" ? " AND " : "";
                    f += " (Date > '" + FromDate.ToString("yyyy-MM-dd") + "') ";
                }

                if (ToDate != null)
                {
                    f += f != "" ? " AND " : "";
                    f += " (Date < '" + ToDate.ToString("yyyy-MM-dd") + "') ";
                }

                return f;
            }
        }

        /// <summary>
        /// Собранная из полей инструкция фильтра коллекции транзакций
        /// </summary>
        public string ViewString
        {
            get
            {

                string f = "";
                if (Acc != null) f += "'" + Acc.Name + "' ";

                if (FromDate != null)
                    f += "С:" + FromDate.ToString("yyyy-MM-dd") + " ";

                if (ToDate != null)
                    f += "По:" + ToDate.ToString("yyyy-MM-dd") + " ";

                return f;
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }//class Filter
}
