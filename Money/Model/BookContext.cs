using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Data.Common;

namespace Money.Model
{
    
    public partial class BookContext : DbContext
    {
        DbConnection Connection;
        public BookContext(DbConnection Connection)
            : base(Connection, true)
        {
            this.Connection = Connection;
            //Connection.GetType()==typeof(SQLiteConnection)
            //Connection.GetType()==typeof(MySql.Data.MySqlClient.MySqlConnection)
            Database.Log = s => Debug.WriteLine(s);
            
        }
        
        /// <summary>
        /// Разного рода ограничения колонок и связи таблиц.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (Connection.GetType() == typeof(SQLiteConnection))
                Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<BookContext>(modelBuilder));
            
            modelBuilder.Entity<Acc>().HasKey(p => p.Id);
            modelBuilder.Entity<Acc>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            modelBuilder.Entity<Acc>().Property(p => p.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Acc>().Property(p => p.Description).HasMaxLength(512);
            modelBuilder.Entity<Acc>().HasMany(a => a.TransOrigin).WithRequired(t=>t.AccOrigin);
            modelBuilder.Entity<Acc>().HasMany(a => a.TransDest).WithRequired(t => t.AccDest); ;

            modelBuilder.Entity<Gp>().HasKey(p => p.Id);
            modelBuilder.Entity<Gp>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            modelBuilder.Entity<Gp>().Property(p => p.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Gp>().Property(p => p.Description).HasMaxLength(512);
            modelBuilder.Entity<Gp>().HasMany(g => g.Accounts).WithMany(a=>a.Gps);

            modelBuilder.Entity<Tran>().HasKey(p => p.Id);
            modelBuilder.Entity<Tran>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            modelBuilder.Entity<Tran>().Property(p => p.Description).HasMaxLength(512);
            modelBuilder.Entity<Tran>().Property(p => p.Date).IsRequired();
            modelBuilder.Entity<Tran>().Property(p => p.Amount).IsRequired();
            modelBuilder.Entity<Tran>().HasRequired(p => p.AccDest).WithMany(a => a.TransDest).HasForeignKey(t=>t.AccDest_Id);
            modelBuilder.Entity<Tran>().HasRequired(p => p.AccOrigin).WithMany(a=>a.TransOrigin).HasForeignKey(t=>t.AccOrigin_Id);

        }

        private DbSet<Acc> accs;
        public virtual DbSet<Acc> Accs
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

        private DbSet<Gp> gps;
        public virtual DbSet<Gp> Gps
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

        private DbSet<Tran> trans;
        public virtual DbSet<Tran> Trans
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

        IQueryable<AccSubTotal> AccDeb
        { get
            {
                return from t in Trans
                       group t by t.AccDest into gD
                       select new AccSubTotal
                       {
                           Acc = gD.Key,
                           Amount = gD.Sum(t => t.Amount)
                       };
            }
        }

        IQueryable<AccSubTotal> AccCred
        {
            get
            {
                return from t in Trans
                       group t by t.AccOrigin into gO
                       select new AccSubTotal
                       {
                           Acc = gO.Key,
                           Amount = gO.Sum(t => t.Amount)
                       };
            }
        }

        public IQueryable<AccSubTotal> AccSummary
        {
            get
            {
                return from a in accs
                       join d in AccDeb on a equals d.Acc into accsD
                       from ad in accsD.DefaultIfEmpty()
                       join o in AccCred on a equals o.Acc into accO
                       from ao in accO.DefaultIfEmpty()
                       select new AccSubTotal
                       {
                           Acc = a,
                           Amount = (((double?)ad.Amount) ?? 0.0) - (((double?)ao.Amount) ?? 0.0)
                       };

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
