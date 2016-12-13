using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace Money.Model
{
    public class DB : INotifyPropertyChanged
    {      
        
        private string sQLiteDataSource = "c:\\temp\\abook.sqlite3";

        private string mySqlServer = "localhost";

        private string mySqlUserID = "root";

        private string mySqlPassword = "";
        
        private string mySqlDatabase = "abook";

        [XmlAttribute]
        public string SQLiteDataSource
        {
            get
            {
                return sQLiteDataSource;
            }

            set
            {
                sQLiteDataSource = value;
                NotifyPropertyChanged();
            }
        }

        [XmlAttribute]
        public string MySqlServer
        {
            get
            {
                return mySqlServer;
            }

            set
            {
                mySqlServer = value;
                NotifyPropertyChanged();
            }
        }

        [XmlAttribute]
        public string MySqlUserID
        {
            get
            {
                return mySqlUserID;
            }

            set
            {
                mySqlUserID = value;
                NotifyPropertyChanged();
            }
        }

        [XmlAttribute]
        public string MySqlPassword
        {
            get
            {
                return mySqlPassword;
            }

            set
            {
                mySqlPassword = value;
                NotifyPropertyChanged();
            }
        }

        [XmlAttribute]
        public string MySqlDatabase
        {
            get
            {
                return mySqlDatabase;
            }

            set
            {
                mySqlDatabase = value;
                NotifyPropertyChanged();
            }
        }

        [XmlIgnore]
        public MySqlConnection MySqlConn
        {
            get
            {
                string cs = string.Format("server = {0}; user id = {1}; password = {2}; persistsecurityinfo = True; database = {3}; charset=utf8",
                        MySqlServer,
                        MySqlUserID,
                        MySqlPassword,
                        MySqlDatabase);
                return new MySqlConnection(cs);

            }
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
