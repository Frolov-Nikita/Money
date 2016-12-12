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

        private List<Tran> trans;
        public List<Tran> Trans
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
        
        #region newRowFields
        private DateTime newDate = DateTime.Now;
        public DateTime NewDate
        {
            get
            {
                return newDate;
            }

            set
            {
                newDate = value;
                NotifyPropertyChanged();
            }
        }

        private int newIDAccOrigin;
        public int NewIDAccOrigin
        {
            get
            {
                return newIDAccOrigin;
            }

            set
            {
                newIDAccOrigin = value;
                NotifyPropertyChanged();
            }
        }

        private int newIDAccDest;
        public int NewIDAccDest
        {
            get
            {
                return newIDAccDest;
            }

            set
            {
                newIDAccDest = value;
                NotifyPropertyChanged();
            }
        }

        private double newAmount = 0.0;
        public double NewAmount
        {
            get
            {
                return newAmount;
            }

            set
            {
                newAmount = value;
                NotifyPropertyChanged();
            }
        }

        private string newDescription = "";
        public string NewDescription
        {
            get
            {
                return newDescription;
            }

            set
            {
                newDescription = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region фильтры
        Acc filterSlectedAcc;
        public Acc FilterAcc
        {
            get { return filterSlectedAcc; }
            set
            {
                filterSlectedAcc = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        private DateTime filterDateBegin = DateTime.Now.AddDays(-30);
        public DateTime FilterDateBegin
        {
            get { return filterDateBegin; }
            set
            {
                filterDateBegin = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        private DateTime filterDateEnd = DateTime.Now.AddDays(1);

        public DateTime FilterDateEnd
        {
            get { return filterDateEnd; }
            set
            {
                filterDateEnd = value;
                FilterEnabled = true;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        private bool filterEnabled;
        //TODO переделать Фильтр на красиво
        public bool FilterEnabled
        {
            get { return filterEnabled; }
            set
            {
                filterEnabled = value;
                FilterUpdate();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Собранная из полей инструкция фильтра коллекции транзакций
        /// </summary>
        public string FilterSQLString { get
            {

                string f = "";
                if (FilterAcc != null) f += " (AccOrigin.Name = '" + FilterAcc.Name + "' OR AccDest.Name = '" + FilterAcc.Name + "') ";

                if (FilterDateBegin != null)
                {
                    f += f != "" ? " AND " : "";
                    f += " (Date > '" + FilterDateBegin.ToString("yyyy-MM-dd") + "') ";
                }

                if (filterDateEnd != null)
                {
                    f += f != "" ? " AND " : "";
                    f += " (Date < '" + filterDateEnd.ToString("yyyy-MM-dd") + "') ";
                }

                return f;
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
        public CommandRef AddNewTransCmd { get; set; }
        public CommandRef RefreshCmd { get; set; }

        public CommandRef AddNewAccCmd { get; set; }
        public CommandRef RemoveAccCmd { get; set; }
        public CommandRef EditAccCmd { get; set; }
        #endregion

        // конструктор
        public WindowWBViewModel()
        {
            BookData = new Model.BookContext();
            
            #region комманды
            AddNewTransCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
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

                    Refresh();
                },
                CanExecuteDelegate = (a) =>
                {
                    return (NewIDAccOrigin > 0) && (NewIDAccDest > 0) && (NewAmount != 0);
                }
            };

            RefreshCmd = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    Refresh();
                },
                //CanExecuteDelegate = (a) => { return BookData.TransHaveChanges; }
            };

            AddNewAccCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    EditAccViewModel EAVM = new EditAccViewModel()
                    {
                        Acc = new Acc(),
                        Gps = Gps
                    };
                }
            };

            RemoveAccCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    Acc A = ((Acc)a);
                    if ((A.TransDest.Count > 0) || (A.TransOrigin.Count > 0))
                    {
                        if (System.Windows.MessageBox.Show("Удаление этого счета приведет\n к потере " +
                            A.TransOrigin.Count + A.TransDest.Count + " транзакций. Все равно удалить?",
                            "ВАЖНО!", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                        {
                            Accs.Remove(A);
                            Refresh();
                        }
                    }
                    else
                    {
                        Accs.Remove(A);
                        Refresh();
                    }
                }
            };

            EditAccCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    EditAccViewModel EAVM = new EditAccViewModel() {
                        Acc = (Acc)a,
                        Gps = Gps
                    };
                    View.EditAcc EA = new View.EditAcc() { DataContext = EAVM};
                    EA.Show();

                }
            };
            #endregion

            Refresh();
        }//public MainWindowModelView()

        /// <summary>
        /// Сохраняет внесенные изменения, редактирует фильтр
        /// </summary>
        public void Refresh()
        {
            BookData.SaveChanges();

            //BookData.AccSummary.ToList

            BookData.Accs.Load();
            BookData.Trans.Load();
            BookData.Gps.Load();
            BookData.AccSummary.Load();

            Accs = BookData.Accs.Local.ToBindingList();
            Gps = BookData.Gps.Local.ToBindingList();

            if (FilterAcc == null)
            {
                Trans = (from t in BookData.Trans.Local
                         where (t.Date >= filterDateBegin) && (t.Date <= filterDateEnd)
                         select t).ToList();
            }
            else
            {
                Trans = (from t in BookData.Trans.Local
                         where (t.Date >= filterDateBegin) &&
                            (t.Date <= filterDateEnd) &&
                            ((t.AccDest == FilterAcc)||(t.AccOrigin == FilterAcc))
                         select t).ToList();
            }

            Accsumms.Clear();
            foreach(var i in BookData.AccSummary.ToList())
                Accsumms.Add(i);

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
