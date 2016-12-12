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
         
        //private Regex r = new Regex("");

        string _Acc="";

        public string Acc
        {
            get
            {
                return _Acc;
            }

            set
            {
                _Acc = value;
                NotifyPropertyChanged();
            }
        }

        string _GP="";

        public string GP
        {
            get
            {
                return _GP;
            }

            set
            {
                _GP = value;
                NotifyPropertyChanged();
            }
        }

        DateTime _FromDate;

        public DateTime FromDate
        {
            get
            {
                return _FromDate;
            }

            set
            {
                _FromDate = value;
                NotifyPropertyChanged();
            }
        }

        DateTime _ToDate;

        public DateTime ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                _ToDate = value;
                NotifyPropertyChanged();
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
            }
        }
        
        public string Rsoult
        {
            get { return ToString(); }
            set { Parse(value); }
        }
        
        void Parse(string sFilter)
        {
            string S = sFilter.Replace("\t", " ").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            while (S.IndexOf(": ") != -1) S = S.Replace(": ",":");
            //int index = 0;

            string[] parts = sFilter.Split(' ');

        }

        public override string ToString()
        {
            string F = "";
            if (GP != "") Acc = "";//доп защита

            if (GP != "") F += "GP:" + GP;
            if (Acc != "") F += "Acc:" + Acc;

            if (HashTag != "")
            {
                F += (F == "") ? "" : " ";
                F += HashTag;
            }

            if (_FromDate.Ticks != 0)
            {
                F += (F == "") ? "" : " " ;
                F += "From:" + _FromDate.ToString("dd.MM.YYYY");
            }

            if (_ToDate.Ticks != 0)
            {
                F += (F == "") ? "" : " ";
                F += "To:" + _ToDate.ToString("dd.MM.YYYY");
            }

            return F;
        }

        public string ToQueryFilterString()
        {
            //TODO Перепилить на SQL синтаксис
            string F = "";
            if (GP != "") Acc = "";//доп защита

            if (GP != "") F += "GP:" + GP;
            if (Acc != "") F += "Acc:" + Acc;

            if (HashTag != "")
            {
                F += (F == "") ? "" : " ";
                F += HashTag;
            }

            if (_FromDate.Ticks != 0)
            {
                F += (F == "") ? "" : " ";
                F += "From:" + _FromDate.ToString("dd.MM.YYYY");
            }

            if (_ToDate.Ticks != 0)
            {
                F += (F == "") ? "" : " ";
                F += "To:" + _ToDate.ToString("dd.MM.YYYY");
            }
            throw new NotImplementedException("Не написана реализация этого метода");
            return F;
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
