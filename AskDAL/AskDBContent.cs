using OUDAL;
using System.Data.Entity;
using System.Data.SQLite;
using System;
using System.Data.SQLite.EF6;
using System.Reflection;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using MySql.Data.Entity; 
using System.Data.Common;

namespace HealthErpDAL
{
    [DbConfigurationType(typeof(SQLiteConfiguration))]
    public class AskDBContent : DbContext
    {
        public static string DbFile = "App_Data/ask.db";
        public AskDBContent() : base(new SQLiteConnection()
        {

            ConnectionString = new SQLiteConnectionStringBuilder()
            {//MyAppContext.RootPath + DbFile,//
                DataSource = MyAppContext.RootPath + DbFile, 
                ForeignKeys = true
            }.ConnectionString
        }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//
            Database.SetInitializer<AskDBContent>(null);
            base.OnModelCreating(modelBuilder);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public void SetDebugLog()
        {
            Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
        }

        public DbSet<AskContent> AskContent { get; set; }
        public DbSet<AskPage> AskPage { get; set; }
        public DbSet<AskResult> AskResult { get; set; }
        public DbSet<AskTopic> AskTopic { get; set; }
    }

    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            Type t = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
            FieldInfo fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            SetProviderServices("System.Data.SQLite", (System.Data.Entity.Core.Common.DbProviderServices)fi.GetValue(null));
        }
    }

 
}
