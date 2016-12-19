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
    public class WBViewModel : INotifyPropertyChanged
    {
        // TODO Окно настройки подключения к MySql и резервное копирование в SQLite. Автономная работа без MySQL
        // TODO Переделать Фильтр на красиво
        // TODO Графики
        // TODO Вставка транзакций из буфера обмена (В отдельном окне)

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

        private Filter filter = new Filter();
        public Filter Filter
        {
            get
            {
                return filter;
            }

            set
            {
                filter = value;
                NotifyPropertyChanged();
            }
        }

        #region График
        private bool _ChartEnabled;

        public bool ChartEnabled
        {
            get { return _ChartEnabled; }
            set
            {
                _ChartEnabled = value;
                NotifyPropertyChanged();
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

        #region Команды
        public CommandRef AddNewTransCmd { get; set; }
        public CommandRef RefreshCmd { get; set; }

        public CommandRef AddNewAccCmd { get; set; }
        public CommandRef RemoveAccCmd { get; set; }
        public CommandRef EditAccCmd { get; set; }

        public CommandRef PasteTransCmd { get; set; }
        #endregion

        // конструктор
        public WBViewModel(BookContext context = null )
        {
            BookData = context?? new BookContext(DB.DefaultSQLiteConn);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Accsumms);
            PropertyGroupDescription pgd = new PropertyGroupDescription(null, new Converters.CnvGroupingAccSubTotal());
            view.GroupDescriptions.Add(pgd);
            
            #region Комманды
            AddNewTransCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    BookData.Trans.Add(new Tran()
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
                    View.EditAcc EA = new View.EditAcc() { DataContext = EAVM };
                    EA.ShowDialog();
                    Accs.Add(EAVM.Acc);
                    Refresh();
                }
            };

            RemoveAccCmd = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    Acc A = ((AccSubTotal)a).Acc;
                    if ((A.TransDest.Count > 0) || (A.TransOrigin.Count > 0))
                    {
                        if (System.Windows.MessageBox.Show("Удаление этого счета приведет\n к потере " +
                            (A.TransOrigin.Count + A.TransDest.Count) + " транзакций. Все равно удалить?",
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
                        Acc = ((AccSubTotal)a).Acc,
                        Gps = Gps
                    };
                    View.EditAcc EA = new View.EditAcc() { DataContext = EAVM};
                    EA.ShowDialog();
                    Refresh();
                }
            };

            PasteTransCmd = new CommandRef()
            {
                ExecuteDelegate = a => { }
            };

            #endregion

            Refresh();
        }//public MainWindowModelView()

        /// <summary>
        /// Сохраняет внесенные изменения, редактирует фильтр
        /// </summary>
        public void Refresh()
        {
            int? saveFilterAccId = null;
            if (filter.Acc != null) saveFilterAccId = filter.Acc.Id;

            BookData.SaveChanges();
            
            BookData.AccSummary.Load();

            Accsumms.Clear();
            foreach(var i in BookData.AccSummary.ToList())
                Accsumms.Add(i);

            BookData.Accs.Load();
            BookData.Trans.Load();
            BookData.Gps.Load();

            Accs = BookData.Accs.Local.ToBindingList();
            Gps = BookData.Gps.Local.ToBindingList();
                                     
            if (saveFilterAccId == null)
            {
                Trans = (from t in BookData.Trans.Local
                         where (t.Date >= Filter.FromDate) && (t.Date <= Filter.ToDate)
                         select t).ToList();
            }
            else
            {
                Filter.Acc = Accs.FirstOrDefault(a => a.Id == saveFilterAccId);
                Trans = (from t in BookData.Trans.Local
                         where (t.Date >= Filter.FromDate) &&
                            (t.Date <= Filter.ToDate) &&
                            ((t.AccDest == Filter.Acc)||(t.AccOrigin == Filter.Acc))
                         select t).ToList();
            }

            UpdateChart();

        }// public void Refresh()

        /// <summary>
        /// Обновляем фильтр в TransView и пересоздаются кривые графика, если тот включен
        /// </summary>
        void UpdateChart()
        {
            ChartBalance.Clear();
            ChartExpense.Clear();
            ChartDates.Clear();

            if (filter.Acc == null)
            {
                ChartEnabled = false;
                return;
            }
            if (!ChartEnabled) return;

            double Summ = 0.0;
            double SubSumm = 0.0;
            double SubCred = 0.0;
            double SubDebt = 0.0;

            for (DateTime d = new DateTime(filter.FromDate.Date.Ticks);
                d<= Filter.ToDate.Date;
                d = d.AddDays(1))
            {
                ChartDates.Add(d.ToString("dd.MM.yyyy"));

                SubDebt = (from t in Trans
                           where (t.Date == d) && (Filter.Acc == t.AccDest)
                           select t).Sum(t => t.Amount);

                SubCred = (from t in Trans
                           where (t.Date == d) && (Filter.Acc == t.AccOrigin)
                           select t).Sum(t => t.Amount);

                SubSumm = SubDebt - SubCred;
                Summ += SubSumm;

                ChartExpense.Add(SubSumm);
                ChartBalance.Add(Summ);

                
            }//for (DateTime d 
            
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
