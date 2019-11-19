using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TKProcessor.Models.TK;

namespace TKProcessor.Contexts
{
    public partial class TKContext : DbContext
    {
        public TKContext()
        {
        }

        public TKContext(DbContextOptions<TKContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<DailyTransactionRecord> DailyTransactionRecord { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSetting { get; set; }
        public virtual DbSet<Mapping> Mapping { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<CalendarDay> CalendarDays { get;set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<Leave> Leave { get; set; }
        public virtual DbSet<RawData> RawData { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WorkSchedule> WorkSchedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["TK"].ToString());
            }
        }
    }
}
