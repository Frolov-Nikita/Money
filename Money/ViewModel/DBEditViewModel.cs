using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Money.Model;
using System.Data.SQLite;
using System.ObjSaver;
using System.IO;
using System.Data.Entity;

namespace Money.ViewModel
{
    public class DBEditViewModel : INotifyPropertyChanged
    {
        Saver saver = new Saver();

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

        private string state;
        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                NotifyPropertyChanged();
                System.Threading.Thread.Sleep(0);
            }
        }

        public CommandRef OpenSQLiteDBDialog { get; set; }

        public CommandRef CheckMySqlConnect { get; set; }
        public CommandRef CheckSQLiteConnect { get; set; }
        
        public CommandRef OpenMySql { get; set; }
        public CommandRef OpenSQLite { get; set; }
        public CommandRef MySqlToSQLite { get; set; }
        public CommandRef SQLiteToMySql { get; set; }


        /// <summary>
        /// Конструктор
        /// </summary>
        public DBEditViewModel()
        {
            // Пробуем прочитать настройки
            saver.FileName = getCfgFilePath();
            if (File.Exists(saver.FileName))
            {
                saver.Open(ref db);
                NotifyPropertyChanged("DB");
            }
            db.PropertyChanged += Db_PropertyChanged;

            #region инициализаторы команд
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

            CheckMySqlConnect = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    DB.CheckMySqlConnect();
                }
            };

            CheckSQLiteConnect = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    DB.CheckSQLiteConnect();
                }
            };

            OpenMySql = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    if (DB.CheckMySqlConnect())
                    {
                        BookContext BC = new BookContext(DB.MySqlConn);
                        WBViewModel WBViewModel = new WBViewModel(BC);
                        View.WB WB = new View.WB() { DataContext = WBViewModel };
                        WB.Title = "MySQL";
                        WB.Show();
                        ((View.DBEdit)a).Close();
                    }
                }
            };

            OpenSQLite = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    if (DB.CheckSQLiteConnect())
                    {
                        BookContext BC = new BookContext(DB.SQLiteConn);
                        WBViewModel WBViewModel = new WBViewModel(BC);
                        View.WB WB = new View.WB() { DataContext = WBViewModel };
                        WB.Title = "SQLite";
                        WB.Show();
                        ((View.DBEdit)a).Close();
                    }
                }
            };

            MySqlToSQLite = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    if (DB.CheckMySqlConnect() && DB.CheckMySqlConnect())
                    {
                        MySqlToSQLiteMethod();
                    }
                }
            };

            SQLiteToMySql = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    if (DB.CheckMySqlConnect() && DB.CheckMySqlConnect())
                    {
                        SQLiteToMySqlMethod();
                    }
                }
            };
            #endregion
        }//public DBEditViewModel()

        void SQLiteToMySqlMethod()
        {
            try
            {
                State = "Подключаемся к базам";
                BookContext BCSQLite = new BookContext(db.SQLiteConn);
                BookContext BCMySql = new BookContext(db.MySqlConn);

                BCMySql.Database.ExecuteSqlCommand(@"
                    Delete from Accs;
                    Delete from Trans;
                    Delete from Gps;
                    Delete from GpAccs;
                ");

                State = "Загружаем данные";
                BCMySql.Accs.Load();
                BCMySql.Trans.Load();
                BCMySql.Gps.Load();

                BCSQLite.Accs.Load();
                BCSQLite.Trans.Load();
                BCSQLite.Gps.Load();

                State = "Чистим базу MySQL";
                //BCMySql.Accs.Local.Clear();
                //BCMySql.Trans.Local.Clear();
                //BCMySql.Gps.Local.Clear();

                //var a = BCSQLite.Accs.Local.ToList();
                //var t = BCSQLite.Trans.Local.ToList();
                //var g = BCSQLite.Gps.Local.ToList();

                //BCSQLite.Dispose();

                //BCMySql.Accs.AddRange(a);
                //BCMySql.Gps.AddRange(g);
                //BCMySql.Trans.AddRange(t);

                State = "Заполняем MySQL новыми данными";
                foreach (var a in BCSQLite.Accs.ToList())
                    BCMySql.Accs.Local.Add(new Acc(a));

                foreach (var t in BCSQLite.Trans.ToList())
                    BCMySql.Trans.Local.Add(new Tran(t));

                foreach (var g in BCSQLite.Gps.ToList())
                {
                    Gp gCopy = new Gp(g);
                    foreach (var a in g.Accounts)
                    {
                        Acc a1 = (from ai in BCMySql.Accs.Local
                                where ai.Id == a.Id
                                select ai).FirstOrDefault();
                        if (a1 != null)
                            gCopy.Accounts.Add(a1);
                    }
                    BCMySql.Gps.Local.Add(gCopy); 
                }

                State = "Сохраняем в базу";
                BCMySql.SaveChanges();
                State = "Синхронизация завершена успешно";
                System.Windows.MessageBox.Show("Завершено");

            }
            catch (Exception ex)
            {
                State = "Произошла ошибка при синхронизации";
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        void MySqlToSQLiteMethod()
        {
            try
            {
                State = "Подключаемся к базам";
                BookContext BCSQLite = new BookContext(db.SQLiteConn);

                //State = "Загружаем данные";
                //BCSQLite.Database.ExecuteSqlCommand(@"
                //    Delete from Accs;
                //    Delete from Trans;
                //    Delete from Gps;
                //    Delete from GpAccs;");

                //BCSQLite.Dispose();
                //BCSQLite = new BookContext(db.SQLiteConn);
                BookContext BCMySql = new BookContext(db.MySqlConn);

                BCMySql.SaveChanges();

                BCMySql.Accs.Load();
                BCMySql.Gps.Load();
                BCMySql.Trans.Load();

                BCSQLite.Accs.Load();
                BCSQLite.Gps.Load();
                BCSQLite.Trans.Load();

                State = "Чистим базу SQLite";
                BCSQLite.Accs.Local.Clear();
                BCSQLite.Gps.Local.Clear();
                BCSQLite.Trans.Local.Clear();

                BCSQLite.SaveChanges();

                //var a = BCSQLite.Accs.Local.ToList();
                //var t = BCSQLite.Trans.Local.ToList();
                //var g = BCSQLite.Gps.Local.ToList();

                //BCSQLite.Dispose();

                //BCMySql.Accs.AddRange(a);
                //BCMySql.Gps.AddRange(g);
                //BCMySql.Trans.AddRange(t);

                State = "Заполняем SQLite новыми данными";
                foreach (var a in BCMySql.Accs.ToList())
                    BCSQLite.Accs.Local.Add(new Acc(a));

                foreach (var t in BCMySql.Trans.ToList())
                    BCSQLite.Trans.Local.Add(new Tran(t));

                foreach (var g in BCMySql.Gps.ToList())
                {
                    Gp gCopy = new Gp(g);
                    foreach (var a in g.Accounts)
                    {
                        Acc a1 = (from ai in BCSQLite.Accs.Local
                                  where ai.Id == a.Id
                                  select ai).FirstOrDefault();
                        if (a1 != null)
                            gCopy.Accounts.Add(a1);
                    }
                    BCSQLite.Gps.Local.Add(gCopy);
                }

                State = "Сохраняем в базу";
                BCSQLite.SaveChanges();
                State = "Синхронизация завершена успешно";
                System.Windows.MessageBox.Show("Завершено");
            }
            catch (Exception ex)
            {
                State = "Произошла ошибка при синхронизации";
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
        }


        private void Db_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            saver.Save(db);
        }
        
        /// <summary>
        /// Путь к файлу с настройками
        /// </summary>
        /// <returns></returns>
        string getCfgFilePath()
        {
            string filename = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.SettingsFile);
            if (File.Exists(filename)) return filename;
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Path.GetFileName(filename);
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
