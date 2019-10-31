using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TKProcessor.Models.DP
{
    public partial class DPContext : DbContext
    {
        public DPContext()
        {
        }

        public DPContext(DbContextOptions<DPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<GscuserSecurity> GscuserSecurity { get; set; }
        public virtual DbSet<PayFreqCalendar> PayFreqCalendar { get; set; }
        public virtual DbSet<PayPackage> PayPackage { get; set; }
        public virtual DbSet<PayPackagePayFreqCalendars> PayPackagePayFreqCalendars { get; set; }
        public virtual DbSet<PayrollCode> PayrollCode { get; set; }
        public virtual DbSet<PayrollTrx> PayrollTrx { get; set; }
        public virtual DbSet<PayrollTrxLines> PayrollTrxLines { get; set; }
        public virtual DbSet<PayrollTrxVoid> PayrollTrxVoid { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DP"].ToString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => e.CountryId)
                    .HasName("IX_Company")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreateGljournalsErpPr).HasColumnName("CreateGLJournalsERP_PR");

                entity.Property(e => e.CreateGljournalsErpPymt).HasColumnName("CreateGLJournalsERP_PYMT");

                entity.Property(e => e.CreateGljournalsSibilPr).HasColumnName("CreateGLJournalsSIBIL_PR");

                entity.Property(e => e.CreateGljournalsSibilPymt).HasColumnName("CreateGLJournalsSIBIL_PYMT");

                entity.Property(e => e.CreatePymtJournalsErp).HasColumnName("CreatePymtJournalsERP");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CurrencyId)
                    .IsRequired()
                    .HasColumnName("CurrencyID")
                    .HasMaxLength(5);

                entity.Property(e => e.CurrencySymbol)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.DefaultCompanyAttributeTemplateSeqId).HasColumnName("DefaultCompanyAttributeTemplateSeqID");

                entity.Property(e => e.DefaultEmployeeAttributeTemplateSeqId).HasColumnName("DefaultEmployeeAttributeTemplateSeqID");

                entity.Property(e => e.DefaultPageSize).HasDefaultValueSql("((100))");

                entity.Property(e => e.DefaultPayrollTrxTemplateSeqId).HasColumnName("DefaultPayrollTrxTemplateSeqID");

                entity.Property(e => e.EarliestEffectiveDateAllowed).HasColumnType("datetime");

                entity.Property(e => e.ErpgljournalPrefixPr)
                    .HasColumnName("ERPGLJournalPrefix_PR")
                    .HasMaxLength(20);

                entity.Property(e => e.ErpgljournalPrefixPymt)
                    .HasColumnName("ERPGLJournalPrefix_PYMT")
                    .HasMaxLength(20);

                entity.Property(e => e.ErpgljournalRefPr)
                    .HasColumnName("ERPGLJournalRef_PR")
                    .HasMaxLength(30);

                entity.Property(e => e.ErpgljournalRefPymt)
                    .HasColumnName("ERPGLJournalRef_PYMT")
                    .HasMaxLength(30);

                entity.Property(e => e.ErpgljournalTemplateIdPr)
                    .HasColumnName("ERPGLJournalTemplateID_PR")
                    .HasMaxLength(20);

                entity.Property(e => e.ErpgljournalTemplateIdPymt)
                    .HasColumnName("ERPGLJournalTemplateID_PYMT")
                    .HasMaxLength(20);

                entity.Property(e => e.ErppymtJournalPrefix)
                    .HasColumnName("ERPPymtJournalPrefix")
                    .HasMaxLength(20);

                entity.Property(e => e.ErppymtJournalRef)
                    .HasColumnName("ERPPymtJournalRef")
                    .HasMaxLength(30);

                entity.Property(e => e.ErppymtJournalTemplateId)
                    .HasColumnName("ERPPymtJournalTemplateID")
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NextEmployeeCode).HasMaxLength(20);

                entity.Property(e => e.PasswordFieldFormat).HasDefaultValueSql("((0))");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.Smslocation)
                    .HasColumnName("SMSLocation")
                    .HasMaxLength(100);

                entity.Property(e => e.UpcomingGlpostingSetupEff).HasColumnName("UpcomingGLPostingSetupEff");

                entity.Property(e => e.UseMultitelSms).HasColumnName("UseMultitelSMS");

                entity.Property(e => e.ValidateEmployeeTaxId).HasColumnName("ValidateEmployeeTaxID");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId)
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Label).HasMaxLength(30);
            });

            modelBuilder.Entity<GscuserSecurity>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.ToTable("GSCUserSecurity");

                entity.HasIndex(e => e.ErpuserId);

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.CountryId, e.UserCode })
                    .HasName("IX_GSCUserSecurity")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ErpuserId)
                    .HasColumnName("ERPUserID")
                    .HasMaxLength(276);

                entity.Property(e => e.GscrecordFilterSeqId).HasColumnName("GSCRecordFilterSeqID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword).HasMaxLength(100);
            });

            modelBuilder.Entity<PayFreqCalendar>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.CountryId, e.SeqId })
                    .HasName("IX_PayFreqCalendar")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CoinageSeqId).HasColumnName("CoinageSeqID");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(30);

                entity.Property(e => e.Label).HasMaxLength(30);

                entity.Property(e => e.MinimumNetPay).HasColumnType("money");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RoundingValue).HasColumnType("money");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PayPackage>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.CountryId, e.Code })
                    .HasName("IX_PayPackage")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(30);

                entity.Property(e => e.Label).HasMaxLength(30);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OriginatingCurrencySeqId).HasColumnName("OriginatingCurrencySeqID");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PayPackagePayFreqCalendars>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.PayPackageSeqId, e.PayFreqCalendarSeqId })
                    .HasName("IX_PayPackagePayFreqCalendars")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PayFreqCalendarSeqId).HasColumnName("PayFreqCalendarSeqID");

                entity.Property(e => e.PayPackageSeqId).HasColumnName("PayPackageSeqID");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.PayFreqCalendarSeq)
                    .WithMany(p => p.PayPackagePayFreqCalendars)
                    .HasForeignKey(d => d.PayFreqCalendarSeqId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayPackagePayFreqCalendars_PayFreqCalendar");

                entity.HasOne(d => d.PayPackageSeq)
                    .WithMany(p => p.PayPackagePayFreqCalendars)
                    .HasForeignKey(d => d.PayPackageSeqId)
                    .HasConstraintName("FK_PayPackagePayFreqCalendars_PayPackage");
            });

            modelBuilder.Entity<PayrollCode>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.CountryId, e.Code })
                    .HasName("IX_PayrollCode")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(30);

                entity.Property(e => e.Label).HasMaxLength(30);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayrollTrx>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.CountryId, e.TrxNo })
                    .HasName("IX_PayrollTrx")
                    .IsUnique();

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DefaultAttributeCode).HasMaxLength(20);

                entity.Property(e => e.DefaultExtRefLine)
                    .HasColumnName("DefaultExtRefLINE")
                    .HasMaxLength(20);

                entity.Property(e => e.DefaultExtRefTrx)
                    .HasColumnName("DefaultExtRefTRX")
                    .HasMaxLength(20);

                entity.Property(e => e.DefaultLineDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultPayFreqCalendarCode).HasMaxLength(20);

                entity.Property(e => e.DefaultPayPackageCode).HasMaxLength(20);

                entity.Property(e => e.DefaultPayrollCodeCode).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Label).HasMaxLength(1000);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PostDate).HasColumnType("datetime");

                entity.Property(e => e.RefDate).HasColumnType("datetime");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.VoidDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PayrollTrxLines>(entity =>
            {
                entity.HasKey(e => e.SeqId);

                entity.HasIndex(e => new { e.ExtRefTrx, e.ExtRefLine })
                    .HasName("IX_PayrollTrxLines_ExtRefTRX");

                entity.HasIndex(e => new { e.PayrollTrxSeqId, e.DisplayOrder })
                    .HasName("IX_PayrollTrxLines");

                entity.Property(e => e.SeqId)
                    .HasColumnName("SeqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AttributeCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ExtRefLine)
                    .HasColumnName("ExtRefLINE")
                    .HasMaxLength(20);

                entity.Property(e => e.ExtRefTrx)
                    .HasColumnName("ExtRefTRX")
                    .HasMaxLength(20);

                entity.Property(e => e.InputValue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LineDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PayDayPayrollTrxSeqId).HasColumnName("PayDayPayrollTrxSeqID");

                entity.Property(e => e.PayFreqCalendarCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PayPackageCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PayrollCodeCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PayrollTrxSeqId).HasColumnName("PayrollTrxSeqID");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.VoidDate).HasColumnType("datetime");

                entity.HasOne(d => d.PayrollTrxSeq)
                    .WithMany(p => p.PayrollTrxLines)
                    .HasForeignKey(d => d.PayrollTrxSeqId)
                    .HasConstraintName("FK_PayrollTrxLines_PayrollTrx");
            });

            modelBuilder.Entity<PayrollTrxVoid>(entity =>
            {
                entity.HasKey(e => e.PayrollTrxSeqId);

                entity.ToTable("PayrollTrxVOID");

                entity.HasIndex(e => new { e.CountryId, e.TrxNo })
                    .HasName("IX_PayrollTrxVOID")
                    .IsUnique();

                entity.Property(e => e.PayrollTrxSeqId)
                    .HasColumnName("PayrollTrxSeqID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DefaultAttributeCode).HasMaxLength(20);

                entity.Property(e => e.DefaultExtRefLine)
                    .HasColumnName("DefaultExtRefLINE")
                    .HasMaxLength(20);

                entity.Property(e => e.DefaultExtRefTrx)
                    .HasColumnName("DefaultExtRefTRX")
                    .HasMaxLength(20);

                entity.Property(e => e.DefaultLineDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultPayFreqCalendarCode).HasMaxLength(20);

                entity.Property(e => e.DefaultPayPackageCode).HasMaxLength(20);

                entity.Property(e => e.DefaultPayrollCodeCode).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Label).HasMaxLength(1000);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PostDate).HasColumnType("datetime");

                entity.Property(e => e.RefDate).HasColumnType("datetime");

                entity.Property(e => e.RowVersion).HasDefaultValueSql("((1))");

                entity.Property(e => e.VoidDate).HasColumnType("datetime");
            });
        }
    }
}
