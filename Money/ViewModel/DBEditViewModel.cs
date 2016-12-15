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
        // TODO проверка подключений

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


        public CommandRef OpenSQLiteDBDialog { get; set; }

        public CommandRef OpenSQLite { get; set; }
        public CommandRef OpenMySql { get; set; }
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

            OpenMySql = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    BookContext BC = new BookContext(DB.MySqlConn);
                    WBViewModel WBViewModel = new WBViewModel(BC);
                    View.WB WB = new View.WB() { DataContext = WBViewModel };
                    WB.Show();
                    ((View.DBEdit)a).Close();
                }
            };

            OpenSQLite = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                    BookContext BC = new BookContext(DB.SQLiteConn);
                    WBViewModel WBViewModel = new WBViewModel(BC);
                    View.WB WB = new View.WB() { DataContext = WBViewModel };
                    WB.Show();
                    ((View.DBEdit)a).Close();
                }
            };

            MySqlToSQLite = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                }
            };

            SQLiteToMySql = new CommandRef()
            {
                ExecuteDelegate = (a) => {
                }
            };
            #endregion
        }//public DBEditViewModel()

        void SQLiteToMySqlMethod()
        {
            BookContext BCSQLite = new BookContext(db.SQLiteConn);
            BookContext BCMySql = new BookContext(db.MySqlConn);

            BCMySql.Accs.LoadAsync();
            BCMySql.Trans.LoadAsync();
            BCMySql.Gps.LoadAsync();

            BCSQLite.Accs.LoadAsync();
            BCSQLite.Trans.LoadAsync();
            BCSQLite.Gps.LoadAsync();

            BCMySql.Accs.Local.Clear();
            BCMySql.Trans.Local.Clear();
            BCMySql.Trans.Local.Clear();


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
