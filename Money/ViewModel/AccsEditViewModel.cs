using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Money.Model;
using System.Data;

namespace Money.ViewModel
{
    public class AccsEditVieModel : INotifyPropertyChanged
    {
        /*
        добавлять счета 
        изменять название и прочие свойства
        добавлять в группы
        */
        public BookData BookData { get; set; }
        
        DataRowView _SelectedAcc;

        public CommandRef Save { get; set; }
        public CommandRef Cancel { get; set; }

        public CommandRef AddGP { get; set; }
        public CommandRef RemoveGP { get; set; }
        
        public CommandRef AddGPRel { get; set; }
        public CommandRef RemoveGPRel { get; set; }

        public DataRowView SelectedAcc
        {
            get
            {
                return _SelectedAcc;
            }

            set
            {
                _SelectedAcc = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SelectedAccGP");
            }
        }

        public DataRow[] SelectedAccGP
        {
            get {
                if (SelectedAcc == null) return null;
                return SelectedAcc.Row.GetChildRows("AccRel");

            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public AccsEditVieModel()
        {

            Save = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    BookData.AccGPRelUpdate();
                    ((View.AccsEdit)a).Close();
                }
            };
            Cancel = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    ((View.AccsEdit)a).Close();
                }
            };
            AddGP = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    var newRow = BookData.DS.Tables["AccGP"].NewRow();
                    newRow["Name"] = "New Group";
                    BookData.DS.Tables["AccGP"].Rows.Add(newRow);
                }
            };
            RemoveGP = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    var RelRows = ((DataRowView)a).Row.GetChildRows("GpRel");
                    if (RelRows.Length > 0)
                        if (System.Windows.MessageBox.Show("Группа содержит " + RelRows.Length + " счетов. \n" +
                            "Счета будут разгруппированы.\n" +
                            "Действительно удалить группу?", "Подтвердите", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
                            return;

                    foreach(var r in RelRows)
                        BookData.DS.Tables["AccGPRel"].Rows.Remove(r);

                    BookData.DS.Tables["AccGP"].Rows.Remove(((DataRowView)a).Row);
                    NotifyPropertyChanged("SelectedAccGP");
                }
            };
            AddGPRel = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    var newRelRow = BookData.DS.Tables["AccGPRel"].NewRow();
                    newRelRow["AccID"] = SelectedAcc.Row["ID"];
                    newRelRow["AccGpID"] = ((DataRowView)a).Row["ID"];
                    BookData.DS.Tables["AccGPRel"].Rows.Add(newRelRow);
                    NotifyPropertyChanged("SelectedAccGP");
                },
                CanExecuteDelegate = (a)=>{
                    if (SelectedAcc == null) return false;
                    var RelRow = ((DataRowView)a).Row;
                    return !BookData.DS.Tables["AccGPRel"].Rows.Contains(
                        new object[] { SelectedAcc.Row["ID"], RelRow["ID"] });
                }
            };
            RemoveGPRel = new CommandRef()
            {
                ExecuteDelegate = (a) =>
                {
                    var rView = ((DataRow)a);
                    var r = BookData.DS.Tables["AccGPRel"].Select("AccGpID =" + rView["AccGpID"] + " AND AccId = " + rView["AccID"])[0];
                    BookData.DS.Tables["AccGPRel"].Rows.Remove(r);
                    NotifyPropertyChanged("SelectedAccGP");
                }
            };
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
