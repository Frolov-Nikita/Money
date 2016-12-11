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

namespace Money.ViewModel
{
    public class WindowWBViewModel : INotifyPropertyChanged
    {

        #region свойства
        BookContext BookData;

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

        private ObservableCollection<AccSubTotal> accsumms = new ObservableCollection<AccSubTotal>();
        public ObservableCollection<AccSubTotal> Accsumms
        {
            get
            {
                return accsumms;
            }

            set
            {
                accsumms = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<object> accTotal;
        public CollectionViewSource A { get; set; }
        public IEnumerable<object> AccTotal //IEnumerable<IGrouping<ObservableCollection<Gp>, AccSubTotal>> 
        {
            get
            {
                return accTotal;
            }

            set
            {
                accTotal = value;
                NotifyPropertyChanged();
            }
        }

        private BindingList<Tran> trans;
        public BindingList<Tran> Trans
        {
            get
            {
                return trans;
            }

            set
            {
                trans = value;
                NotifyPropertyChanged();
            }
        }
        
        #region newRowFields
        public DateTime NewDate { get; set; } = DateTime.Now;
        public int NewIDAccOrigin { get; set; }
        public int NewIDAccDest { get; set; }
        public double NewAmount { get; set; }
        public string NewDescription { get; set; } = "";
        #endregion

        #region фильтры
        int _FilterSlectedAcc;
        public int FilterSlectedAcc
        {
            get { return _FilterSlectedAcc; }
            set
            {
                _FilterSlectedAcc = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }


        DateTime _filterDateBegin = DateTime.Now.AddDays(-30);
        public DateTime FilterDateBegin
        {
            get { return _filterDateBegin; }
            set
            {
                _filterDateBegin = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        private DateTime _filterDateEnd = DateTime.Now.AddDays(1);

        public DateTime FilterDateEnd
        {
            get { return _filterDateEnd; }
            set
            {
                _filterDateEnd = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        private bool _filterEnabled;
        //TODO переделать Фильтр на красиво
        public bool FilterEnabled
        {
            get { return _filterEnabled; }
            set
            {
                _filterEnabled = value;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region график
        private bool _ChartEnabled;

        public bool ChartEnabled
        {
            get { return _ChartEnabled; }
            set
            {
                _ChartEnabled = value;
                NotifyPropertyChanged();
                FilterUpdate();
            }
        }

        public SeriesCollection ChartSeriesColl
        {
            get
            {
                return new SeriesCollection {
                    new LineSeries
                    {
                        Title = "Остаток",
                        Values = ChartBalance
                    },
                    new ColumnSeries
                    {
                        Title = "Расходы",
                        Values = ChartExpense
                    }
                };
            }
        }

        public ChartValues<double> ChartBalance { get; set; } = new ChartValues<double>();
        public ChartValues<double> ChartExpense { get; set; } = new ChartValues<double>();
        public List<string> ChartDates { get; set; } = new List<string>();
        #endregion

        #endregion

        #region command
        public CommandRef AddNewTrans { get; set; }
        public CommandRef UpdateTrans { get; set; }


        #endregion

        // конструктор
        public WindowWBViewModel()
        {
            BookData = new Model.BookContext();
            
            #region комманды
            AddNewTrans = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    AddNewTransMethod();
                    Refresh();
                },

                CanExecuteDelegate = (a) =>
                {
                    return (NewIDAccOrigin > 0) && (NewIDAccDest > 0) && (NewAmount != 0);
                }
            };

            UpdateTrans = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    Refresh();
                },
                //CanExecuteDelegate = (a) => { return BookData.TransHaveChanges; }
            };
            #endregion

            Refresh();
        }//public MainWindowModelView()

        public void Refresh()
        {
            BookData.SaveChanges();

            //BookData.AccSummary.ToList

            BookData.Accs.Load();
            BookData.Trans.Load();
            BookData.AccSummary.Load();

            Accs = BookData.Accs.Local.ToBindingList();
            Trans = BookData.Trans.Local.ToBindingList();

            Accsumms.Clear();
            foreach(var i in BookData.AccSummary.ToList())
                Accsumms.Add(i);

        }
        /// <summary>
        /// Метод добавления новой транзакции из полей ввода
        /// </summary>
        void AddNewTransMethod()
        {
            BookData.Trans.Add(new Model.Tran()
            {
                AccDest_Id = NewIDAccDest,
                AccOrigin_Id = NewIDAccOrigin,
                Date = NewDate,
                Description = NewDescription,
                Amount = NewAmount
            });
            
            NewIDAccDest = 0;
            NewAmount = 0.0;
            NewDescription = "";

            NotifyPropertyChanged("NewIDAccDest");
            NotifyPropertyChanged("NewAmount");
            NotifyPropertyChanged("NewDescription");
        }

        /// <summary>
        /// Обновляем фильтр в TransView и пересоздаются кривые графика, если тот включен
        /// </summary>
        void FilterUpdate()
        {
            //if (!FilterEnabled)
            //{
            //    TransView.RowFilter = "";
            //    return;
            //}

            //if (FilterSlectedAcc > 0)
            //    TransView.RowFilter = "(IDAccOrigin = " + FilterSlectedAcc + " OR IDAccDest = " + FilterSlectedAcc + ") AND Date >= '" + FilterDateBegin.ToString() + "' and Date <= '" + FilterDateEnd.ToString() + "'";
            //else
            //    TransView.RowFilter = "Date >= '" + FilterDateBegin.ToString() + "' and Date <= '" + FilterDateEnd.ToString() + "'";
            //// обновление линий тренда, если график включен
            //if (ChartEnabled && (FilterSlectedAcc > 0))
            //{
            //    ChartBalance.Clear();
            //    ChartExpense.Clear();
            //    ChartDates.Clear();
            //    double summ = 0.0, balanceSumm = 0.0;
            //    //TODO переделать графики тут
            //    var groups = TransView.ToTable().AsEnumerable().OrderBy(x => x.Field<DateTime>("Date"))
            //        .GroupBy(r => r.Field<DateTime>("Date"));
            //    foreach (var gp in groups)
            //    {
            //        ChartDates.Add(gp.Key.ToString("dd.MM.yyyy"));
            //        summ = 0;
            //        foreach (var row in gp)
            //        {
            //            if (row.Field<long>("IDAccOrigin") == FilterSlectedAcc)
            //                summ -= row.Field<double>("Amount");
            //            else
            //                summ += row.Field<double>("Amount");
            //        }
            //        balanceSumm += summ;
            //        ChartExpense.Add(summ);
            //        ChartBalance.Add(balanceSumm);
            //    }

            //}

            ////NotifyPropertyChanged(ChartSeriesColl)
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
