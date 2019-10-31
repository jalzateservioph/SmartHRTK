using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TKProcessor.Models.SHR;

namespace TKProcessor.Contexts
{
    public partial class SHRContext : DbContext
    {
        public SHRContext()
        {
        }

        public SHRContext(DbContextOptions<SHRContext> options)
            : base(options)
        {
        }

        public virtual DbSet<JobGradeBandLu> JobGradeBandLu { get; set; }
        public virtual DbSet<Leave> Leave { get; set; }
        public virtual DbSet<Personnel> Personnel { get; set; }
        public virtual DbSet<Personnel1> Personnel1 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SHR"].ToString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<JobGradeBandLu>(entity =>
            {
                entity.HasKey(e => new { e.CompanyNum, e.JobGradeBand })
                    .HasName("aaaaaJobGradeBandLU_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("JobGradeBandLU");

                entity.HasIndex(e => e.CompanyNum)
                    .HasName("JobGradeBandLUIdx");

                entity.Property(e => e.CompanyNum).HasMaxLength(20);

                entity.Property(e => e.JobGradeBand).HasMaxLength(50);
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(e => new { e.CompanyNum, e.EmployeeNum, e.StartDate })
                    .HasName("aaaaaLeave_PK")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.LeaveType)
                    .HasName("Reference41");

                entity.HasIndex(e => new { e.CompanyNum, e.EmployeeNum })
                    .HasName("Reference23");

                entity.Property(e => e.CompanyNum).HasMaxLength(50);

                entity.Property(e => e.EmployeeNum)
                    .HasMaxLength(12)
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.CaptureDate).HasColumnType("datetime");

                entity.Property(e => e.CapturedByUsername).HasMaxLength(20);

                entity.Property(e => e.DeclineReason).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Doctor).HasMaxLength(20);

                entity.Property(e => e.Duration).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.EmailSent)
                    .IsRequired()
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LeaveSeq).ValueGeneratedOnAdd();

                entity.Property(e => e.LeaveStatus).HasMaxLength(15);

                entity.Property(e => e.LeaveType).HasMaxLength(50);

                entity.Property(e => e.MedicalCert)
                    .IsRequired()
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.PathId).HasColumnName("PathID");

                entity.Property(e => e.Remarks).HasColumnType("ntext");

                entity.Property(e => e.TemplateCode).HasMaxLength(10);

                entity.Property(e => e.UnitOfMeassure).HasMaxLength(1);
            });

            modelBuilder.Entity<Personnel>(entity =>
            {
                entity.HasKey(e => new { e.CompanyNum, e.EmployeeNum })
                    .HasName("aaaaaPersonnel_PK")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Appointype)
                    .HasName("Appointype_Idx");

                entity.HasIndex(e => e.CompanyNum)
                    .HasName("Reference58");

                entity.HasIndex(e => e.Disability)
                    .HasName("Disability_Idx");

                entity.HasIndex(e => e.EthnicGroup)
                    .HasName("Reference11");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Language)
                    .HasName("Reference22");

                entity.HasIndex(e => e.MaritialStatus)
                    .HasName("Reference26");

                entity.HasIndex(e => e.Nationality)
                    .HasName("Reference27");

                entity.HasIndex(e => e.Religion)
                    .HasName("Reference30");

                entity.HasIndex(e => e.Sex)
                    .HasName("Reference45");

                entity.HasIndex(e => e.SkillLevel)
                    .HasName("SkillLevel_Idx");

                entity.HasIndex(e => e.TerminationReason)
                    .HasName("TermReasonLUPersonnelRelation");

                entity.HasIndex(e => new { e.CompanyNum, e.CostCentre })
                    .HasName("CompCostCentre_Idx");

                entity.HasIndex(e => new { e.CompanyNum, e.JobGrade })
                    .HasName("JobGradePersonnel");

                entity.HasIndex(e => new { e.CompanyNum, e.JobTitle })
                    .HasName("JobTitlePersonnel");

                entity.HasIndex(e => new { e.CompanyNum, e.LeaveScheme })
                    .HasName("Leave_SchemesPersonnel");

                entity.HasIndex(e => new { e.DeptName, e.CompanyNum })
                    .HasName("DepartmentPersonnel");

                entity.Property(e => e.CompanyNum).HasMaxLength(20);

                entity.Property(e => e.EmployeeNum)
                    .HasMaxLength(12)
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.AccName).HasMaxLength(30);

                entity.Property(e => e.AccNum).HasMaxLength(20);

                entity.Property(e => e.AddrCity).HasMaxLength(80);

                entity.Property(e => e.AddrComplex).HasMaxLength(80);

                entity.Property(e => e.AddrCountryCode).HasMaxLength(3);

                entity.Property(e => e.AddrState).HasMaxLength(50);

                entity.Property(e => e.AddrStreetName).HasMaxLength(80);

                entity.Property(e => e.AddrStreetNo).HasMaxLength(20);

                entity.Property(e => e.AddrSuburb).HasMaxLength(80);

                entity.Property(e => e.AddrUnit).HasMaxLength(30);

                entity.Property(e => e.AddrZip).HasMaxLength(10);

                entity.Property(e => e.Address1).HasMaxLength(80);

                entity.Property(e => e.Address2).HasMaxLength(80);

                entity.Property(e => e.Address3).HasMaxLength(80);

                entity.Property(e => e.Address4).HasMaxLength(50);

                entity.Property(e => e.Appointdate).HasColumnType("datetime");

                entity.Property(e => e.Appointype).HasMaxLength(15);

                entity.Property(e => e.Bank).HasMaxLength(30);

                entity.Property(e => e.BankCode).HasMaxLength(10);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Branch).HasMaxLength(30);

                entity.Property(e => e.CellTel).HasMaxLength(50);

                entity.Property(e => e.ClockNumber).HasMaxLength(15);

                entity.Property(e => e.CostCentre)
                    .HasMaxLength(50)
                    .HasDefaultValueSql(@"/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
/*                         V E R S I O N   9 . 5                              */
/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
CREATE DEFAULT Default_Dash AS '-'


");

                entity.Property(e => e.CountryCode).HasMaxLength(3);

                entity.Property(e => e.DateJoinedGroup).HasColumnType("datetime");

                entity.Property(e => e.DeptName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.Personnel_DeptName_D    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.Personnel_DeptName_D    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.Personnel_DeptName_D    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.Personnel_DeptName_D    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.Personnel_DeptName_D    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.Personnel_DeptName_D AS '-'








");

                entity.Property(e => e.Disability).HasMaxLength(50);

                entity.Property(e => e.DisabilityNotes).HasMaxLength(255);

                entity.Property(e => e.DpAttributeCode)
                    .HasColumnName("DP_AttributeCode")
                    .HasMaxLength(20);

                entity.Property(e => e.DpPayrollCode)
                    .HasColumnName("DP_PayrollCode")
                    .HasMaxLength(20);

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("EMailAddress")
                    .HasMaxLength(80);

                entity.Property(e => e.EmailAddress1)
                    .HasColumnName("EMailAddress1")
                    .HasMaxLength(50);

                entity.Property(e => e.EmployeePicture).HasColumnType("image");

                entity.Property(e => e.EmploymentContractType).HasMaxLength(50);

                entity.Property(e => e.EmploymentEndDate).HasColumnType("datetime");

                entity.Property(e => e.EmploymentType)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Esspath)
                    .HasColumnName("ESSPath")
                    .HasMaxLength(256);

                entity.Property(e => e.EthnicGroup).HasMaxLength(50);

                entity.Property(e => e.ExtensionNo).HasMaxLength(30);

                entity.Property(e => e.FaxNo).HasMaxLength(30);

                entity.Property(e => e.FirstName).HasMaxLength(80);

                entity.Property(e => e.Flag).HasMaxLength(2);

                entity.Property(e => e.HomeTel).HasMaxLength(50);

                entity.Property(e => e.Hr)
                    .IsRequired()
                    .HasColumnName("HR")
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Idnum)
                    .HasColumnName("IDNum")
                    .HasMaxLength(50);

                entity.Property(e => e.Induction)
                    .IsRequired()
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Initials).HasMaxLength(5);

                entity.Property(e => e.JobGrade)
                    .HasMaxLength(50)
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.Personnel_JobGrade_D    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.Personnel_JobGrade_D    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.Personnel_JobGrade_D    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.Personnel_JobGrade_D    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.Personnel_JobGrade_D    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.Personnel_JobGrade_D AS '-'








");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(50)
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.Personnel_JobTitle_D    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.Personnel_JobTitle_D    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.Personnel_JobTitle_D    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.Personnel_JobTitle_D    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.Personnel_JobTitle_D    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.Personnel_JobTitle_D AS '-'








");

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasColumnType("decimal(10, 7)");

                entity.Property(e => e.LeaveScheme)
                    .HasColumnName("Leave_Scheme")
                    .HasMaxLength(50);

                entity.Property(e => e.Longitude).HasColumnType("decimal(10, 7)");

                entity.Property(e => e.MaidenName).HasMaxLength(80);

                entity.Property(e => e.MaritialStatus).HasMaxLength(25);

                entity.Property(e => e.MiddleName).HasMaxLength(80);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NoticePeriod).HasMaxLength(20);

                entity.Property(e => e.OfficeNo).HasMaxLength(50);

                entity.Property(e => e.PathId).HasColumnName("PathID");

                entity.Property(e => e.PayMode).HasMaxLength(20);

                entity.Property(e => e.PayPoint).HasMaxLength(20);

                entity.Property(e => e.Payday).HasColumnType("datetime");

                entity.Property(e => e.Payenum)
                    .HasColumnName("PAYENum")
                    .HasMaxLength(20);

                entity.Property(e => e.PayrollEmployeeNum).HasMaxLength(12);

                entity.Property(e => e.PayrollNumber).HasMaxLength(20);

                entity.Property(e => e.PictureFile).HasMaxLength(255);

                entity.Property(e => e.Poarea)
                    .HasColumnName("POArea")
                    .HasMaxLength(80);

                entity.Property(e => e.Pobox)
                    .HasColumnName("POBox")
                    .HasMaxLength(80);

                entity.Property(e => e.Pocode)
                    .HasColumnName("POCode")
                    .HasMaxLength(50);

                entity.Property(e => e.Postate)
                    .HasColumnName("POState")
                    .HasMaxLength(80);

                entity.Property(e => e.PreferredName).HasMaxLength(50);

                entity.Property(e => e.PrivateBag).HasMaxLength(30);

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.Read)
                    .IsRequired()
                    .HasColumnName("Read_")
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Recruit).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.RecruitStatus).HasMaxLength(1);

                entity.Property(e => e.Religion).HasMaxLength(15);

                entity.Property(e => e.ReligionNotes).HasMaxLength(255);

                entity.Property(e => e.Resigned).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Sex).HasMaxLength(20);

                entity.Property(e => e.SkillLevel)
                    .HasColumnName("Skill_Level")
                    .HasMaxLength(50);

                entity.Property(e => e.SpouseName).HasMaxLength(80);

                entity.Property(e => e.SpouseTel).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(15);

                entity.Property(e => e.StreetState).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(80);

                entity.Property(e => e.SwiftCode).HasMaxLength(20);

                entity.Property(e => e.TaxCode).HasMaxLength(20);

                entity.Property(e => e.TaxNum).HasMaxLength(20);

                entity.Property(e => e.TaxOffice).HasMaxLength(30);

                entity.Property(e => e.Termination)
                    .IsRequired()
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.TerminationDate).HasColumnType("datetime");

                entity.Property(e => e.TerminationReason).HasMaxLength(80);

                entity.Property(e => e.Title).HasMaxLength(6);

                entity.Property(e => e.TodateSaved)
                    .HasColumnName("TODateSaved")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tousername)
                    .HasColumnName("TOUsername")
                    .HasMaxLength(20);

                entity.Property(e => e.TransferDate).HasColumnType("datetime");

                entity.Property(e => e.TransferReason).HasMaxLength(80);

                entity.Property(e => e.Uifnum)
                    .HasColumnName("UIFNum")
                    .HasMaxLength(20);

                entity.Property(e => e.Worktel)
                    .HasColumnName("worktel")
                    .HasMaxLength(50);

                entity.Property(e => e.Write)
                    .IsRequired()
                    .HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");
            });

            modelBuilder.Entity<Personnel1>(entity =>
            {
                entity.HasKey(e => new { e.CompanyNum, e.EmployeeNum })
                    .HasName("aaaaaPersonnel1_PK")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.EthnicBackground)
                    .HasName("PErsonnel1EthnicBackgroundIdx");

                entity.HasIndex(e => e.GradeBand)
                    .HasName("Personnel1OccupLvlIdx");

                entity.HasIndex(e => e.Position)
                    .HasName("{98AF1DDD-49C9-4799-BBB7-C1616468376F}");

                entity.HasIndex(e => e.SalaryIncGridCode)
                    .HasName("{C3EFFB18-B34A-44F3-B029-D9CABF2A0B53}");

                entity.HasIndex(e => e.SkillCategory)
                    .HasName("Personnel1SkillCatIdx");

                entity.HasIndex(e => new { e.CompanyNum, e.JobGradeBand })
                    .HasName("Personnel1GradeBandIdx");

                entity.HasIndex(e => new { e.CompanyNum, e.Location })
                    .HasName("LocationLUPersonnel1");

                entity.HasIndex(e => new { e.CompanyNum, e.ShiftType })
                    .HasName("ShiftTypeLUPersonnel1");

                entity.Property(e => e.CompanyNum).HasMaxLength(20);

                entity.Property(e => e.EmployeeNum).HasMaxLength(12);

                entity.Property(e => e.AccountCurrency).HasMaxLength(3);

                entity.Property(e => e.Adusername)
                    .HasColumnName("ADUsername")
                    .HasMaxLength(30);

                entity.Property(e => e.BlackListDate).HasColumnType("datetime");

                entity.Property(e => e.BlackListed).HasDefaultValueSql("(0)");

                entity.Property(e => e.BlacklistComments).HasMaxLength(1000);

                entity.Property(e => e.Breadwinner).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Citizenship).HasMaxLength(50);

                entity.Property(e => e.CountryOfBirth).HasMaxLength(80);

                entity.Property(e => e.CreditCheckDate).HasColumnType("datetime");

                entity.Property(e => e.CreditCheckRemarks).HasMaxLength(255);

                entity.Property(e => e.DateNoticeHandedIn).HasColumnType("datetime");

                entity.Property(e => e.DaysPerWeek).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.DepartmentSection).HasMaxLength(50);

                entity.Property(e => e.DoEmploy).HasDefaultValueSql("((0))");

                entity.Property(e => e.EthnicBackground).HasMaxLength(30);

                entity.Property(e => e.GradeBand).HasMaxLength(50);

                entity.Property(e => e.Idtype)
                    .HasColumnName("IDType")
                    .HasMaxLength(20);

                entity.Property(e => e.IndividualJobTitle).HasMaxLength(100);

                entity.Property(e => e.JobFunction).HasMaxLength(50);

                entity.Property(e => e.JobGradeBand)
                    .HasMaxLength(50)
                    .HasDefaultValueSql(@"/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
/*                         V E R S I O N   9 . 5                              */
/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
CREATE DEFAULT Default_Dash AS '-'


");

                entity.Property(e => e.LastWorkingDay).HasColumnType("datetime");

                entity.Property(e => e.Location)
                    .HasMaxLength(40)
                    .HasDefaultValueSql(@"/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
/*                         V E R S I O N   9 . 5                              */
/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
CREATE DEFAULT Default_Dash AS '-'


");

                entity.Property(e => e.LocationCategory).HasMaxLength(50);

                entity.Property(e => e.MarriageContract).HasMaxLength(30);

                entity.Property(e => e.MarriageDate).HasColumnType("datetime");

                entity.Property(e => e.NumberOfHours).HasDefaultValueSql(@"/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 24/01/2000 8:26:13 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 27/11/1999 6:43:37 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 16/10/99 4:43:20 AM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 06/10/99 7:58:18 PM ******/
/****** Object:  Default dbo.UW_ZeroDefault    Script Date: 02/10/99 6:54:00 AM ******/
CREATE DEFAULT dbo.UW_ZeroDefault AS 0








");

                entity.Property(e => e.Ofocode)
                    .HasColumnName("OFOCode")
                    .HasMaxLength(15);

                entity.Property(e => e.Ofooccupation)
                    .HasColumnName("OFOOccupation")
                    .HasMaxLength(100);

                entity.Property(e => e.OtherCheck).HasMaxLength(50);

                entity.Property(e => e.OtherCheckRemarks).HasMaxLength(255);

                entity.Property(e => e.PoliceClearanceChkDate).HasColumnType("datetime");

                entity.Property(e => e.PoliceClearanceRemarks).HasMaxLength(255);

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.ProbationEndDate).HasColumnType("datetime");

                entity.Property(e => e.ResignationReason).HasMaxLength(50);

                entity.Property(e => e.SalaryIncGridCode).HasMaxLength(10);

                entity.Property(e => e.ShiftType)
                    .HasMaxLength(40)
                    .HasDefaultValueSql(@"/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
/*                         V E R S I O N   9 . 5                              */
/******************************************************************************/
/******************************************************************************/
/******************************************************************************/
CREATE DEFAULT Default_Dash AS '-'


");

                entity.Property(e => e.SkillCategory).HasMaxLength(35);

                entity.Property(e => e.SpouseDob)
                    .HasColumnName("SpouseDOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.SpouseId)
                    .HasColumnName("SpouseID")
                    .HasMaxLength(20);

                entity.Property(e => e.SpouseMaidenName).HasMaxLength(80);

                entity.Property(e => e.SpousePassport).HasMaxLength(20);

                entity.Property(e => e.SpouseSurname).HasMaxLength(80);

                entity.Property(e => e.SpouseTitle).HasMaxLength(6);

                entity.Property(e => e.TaxCountryCode).HasMaxLength(3);

                entity.Property(e => e.TaxState).HasMaxLength(50);

                entity.Property(e => e.TerminationRemarks).HasMaxLength(255);

                entity.Property(e => e.TownOfBirth).HasMaxLength(80);

                entity.Property(e => e.WithdrawalDate).HasColumnType("datetime");

                entity.Property(e => e.WithdrawalReason).HasMaxLength(80);
            });
        }
    }
}
