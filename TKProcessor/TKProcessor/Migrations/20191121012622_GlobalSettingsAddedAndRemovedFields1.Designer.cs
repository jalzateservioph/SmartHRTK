﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TKProcessor.Contexts;

namespace TKProcessor.Migrations
{
    [DbContext(typeof(TKContext))]
    [Migration("20191121012622_GlobalSettingsAddedAndRemovedFields1")]
    partial class GlobalSettingsAddedAndRemovedFields1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TKProcessor.Models.TK.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("NewValue");

                    b.Property<string>("OldValue");

                    b.Property<string>("Target");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Calendar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.CalendarDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsLegalHoliday");

                    b.Property<bool>("IsSpecialHoliday");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("CalendarDays");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.DailyTransactionRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AbsentHours");

                    b.Property<decimal>("ActualLate");

                    b.Property<decimal>("ActualLegHol");

                    b.Property<decimal>("ActualLegHolOt");

                    b.Property<decimal>("ActualLegHolRD");

                    b.Property<decimal>("ActualLegHolRDot");

                    b.Property<decimal>("ActualLegSpeHol");

                    b.Property<decimal>("ActualLegSpeHolOt");

                    b.Property<decimal>("ActualLegSpeHolRD");

                    b.Property<decimal>("ActualLegSpeHolRDot");

                    b.Property<decimal>("ActualNDLegHol");

                    b.Property<decimal>("ActualNDLegHolOt");

                    b.Property<decimal>("ActualNDLegHolRD");

                    b.Property<decimal>("ActualNDLegHolRDot");

                    b.Property<decimal>("ActualNDLegSpeHol");

                    b.Property<decimal>("ActualNDLegSpeHolOt");

                    b.Property<decimal>("ActualNDLegSpeHolRD");

                    b.Property<decimal>("ActualNDLegSpeHolRDot");

                    b.Property<decimal>("ActualNDRD");

                    b.Property<decimal>("ActualNDRDot");

                    b.Property<decimal>("ActualNDSpeHol");

                    b.Property<decimal>("ActualNDSpeHolOt");

                    b.Property<decimal>("ActualNDSpeHolRD");

                    b.Property<decimal>("ActualNDSpeHolRDot");

                    b.Property<decimal>("ActualOvertime");

                    b.Property<decimal>("ActualPostOvertime");

                    b.Property<decimal>("ActualPreOvertime");

                    b.Property<decimal>("ActualRestDay");

                    b.Property<decimal>("ActualRestDayOt");

                    b.Property<decimal>("ActualSpeHol");

                    b.Property<decimal>("ActualSpeHolOt");

                    b.Property<decimal>("ActualSpeHolRD");

                    b.Property<decimal>("ActualSpeHolRDot");

                    b.Property<decimal>("ActualUndertime");

                    b.Property<decimal>("ApprovedLate");

                    b.Property<decimal>("ApprovedLegHol");

                    b.Property<decimal>("ApprovedLegHolOt");

                    b.Property<decimal>("ApprovedLegHolRD");

                    b.Property<decimal>("ApprovedLegHolRDot");

                    b.Property<decimal>("ApprovedLegSpeHol");

                    b.Property<decimal>("ApprovedLegSpeHolOt");

                    b.Property<decimal>("ApprovedLegSpeHolRD");

                    b.Property<decimal>("ApprovedLegSpeHolRDot");

                    b.Property<decimal>("ApprovedNDLegHol");

                    b.Property<decimal>("ApprovedNDLegHolOt");

                    b.Property<decimal>("ApprovedNDLegHolRD");

                    b.Property<decimal>("ApprovedNDLegHolRDot");

                    b.Property<decimal>("ApprovedNDLegSpeHol");

                    b.Property<decimal>("ApprovedNDLegSpeHolOt");

                    b.Property<decimal>("ApprovedNDLegSpeHolRD");

                    b.Property<decimal>("ApprovedNDLegSpeHolRDot");

                    b.Property<decimal>("ApprovedNDRD");

                    b.Property<decimal>("ApprovedNDRDot");

                    b.Property<decimal>("ApprovedNDSpeHol");

                    b.Property<decimal>("ApprovedNDSpeHolOt");

                    b.Property<decimal>("ApprovedNDSpeHolRD");

                    b.Property<decimal>("ApprovedNDSpeHolRDot");

                    b.Property<decimal>("ApprovedOvertime");

                    b.Property<decimal>("ApprovedPostOvertime");

                    b.Property<decimal>("ApprovedPreOvertime");

                    b.Property<decimal>("ApprovedRestDay");

                    b.Property<decimal>("ApprovedRestDayOt");

                    b.Property<decimal>("ApprovedSpeHol");

                    b.Property<decimal>("ApprovedSpeHolOt");

                    b.Property<decimal>("ApprovedSpeHolRD");

                    b.Property<decimal>("ApprovedSpeHolRDot");

                    b.Property<decimal>("ApprovedUndertime");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("EmployeeId");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("LeaveType");

                    b.Property<decimal>("NightDifferential");

                    b.Property<decimal>("NightDifferentialOt");

                    b.Property<decimal>("RegularWorkHours");

                    b.Property<string>("Remarks");

                    b.Property<Guid?>("ShiftId");

                    b.Property<DateTime?>("TimeIn");

                    b.Property<DateTime?>("TimeOut");

                    b.Property<DateTime?>("TransactionDate");

                    b.Property<decimal>("WorkHours");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("ShiftId");

                    b.ToTable("DailyTransactionRecord");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BiometricsId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("EmployeeCode");

                    b.Property<string>("FullName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("JobGradeBand");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Password");

                    b.Property<DateTime?>("TerminationDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.EmployeeWorkSite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EmployeeId");

                    b.Property<Guid>("WorkSiteId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("WorkSiteId");

                    b.ToTable("EmployeeWorkSite");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.ErrorLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("DateRaised");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Message");

                    b.Property<string>("Source");

                    b.Property<string>("StackTrace");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("ErrorLog");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.GlobalSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("DefaultNDEnd");

                    b.Property<DateTime>("DefaultNDStart");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("GlobalSetting");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Holiday", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Holiday");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Leave", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("EmployeeCode");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime>("LeaveDate");

                    b.Property<decimal>("LeaveHours");

                    b.Property<string>("LeaveType");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Leave");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Mapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("GlobalSettingId");

                    b.Property<Guid?>("GlobalSettingId1");

                    b.Property<int>("Order");

                    b.Property<string>("Source");

                    b.Property<string>("Target");

                    b.HasKey("Id");

                    b.HasIndex("GlobalSettingId");

                    b.HasIndex("GlobalSettingId1");

                    b.ToTable("Mapping");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.RawData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BiometricsId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("ScheduleDate");

                    b.Property<DateTime?>("TransactionDateTime");

                    b.Property<int?>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("RawData");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.SelectionSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DisplayOrder");

                    b.Property<Guid?>("GlobalSettingId");

                    b.Property<bool>("IsSelected");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("GlobalSettingId");

                    b.ToTable("SelectionSetting");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Shift", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AfterEvery");

                    b.Property<decimal?>("AfterEveryEarlyOut");

                    b.Property<DateTime?>("AmbreakIn");

                    b.Property<DateTime?>("AmbreakOut");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<int?>("DeductionLateIn");

                    b.Property<int?>("DeductionOfEarlyOut");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("DinnerIn");

                    b.Property<DateTime?>("DinnerOut");

                    b.Property<DateTime?>("EarliestTimeIn");

                    b.Property<DateTime?>("EarliestTimeOut");

                    b.Property<int?>("FlextimeType");

                    b.Property<int?>("FocusDate");

                    b.Property<int?>("GracePeriodEarlyOut");

                    b.Property<int?>("GracePeriodLateIn");

                    b.Property<int?>("Increment");

                    b.Property<bool>("IsActive");

                    b.Property<bool?>("IsEarlyOut");

                    b.Property<bool?>("IsHolidayRestDayOt");

                    b.Property<bool?>("IsLateIn");

                    b.Property<bool?>("IsOverbreak");

                    b.Property<bool?>("IsPlusEarlyOutMinutes");

                    b.Property<bool?>("IsPlusLateInMinutes");

                    b.Property<bool?>("IsPostShiftOt");

                    b.Property<bool?>("IsPreShiftOt");

                    b.Property<bool?>("IsRestDay");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime?>("LatestTimeIn");

                    b.Property<DateTime?>("LatestTimeOut");

                    b.Property<DateTime?>("LunchIn");

                    b.Property<DateTime?>("LunchOut");

                    b.Property<int?>("MaximumHolidayRestDayOt");

                    b.Property<int?>("MaximumMinutesConsideredAsHalfAayEarlyOut");

                    b.Property<int?>("MaximumMinutesConsideredAsHalfDay");

                    b.Property<int?>("MaximumPostShiftOt");

                    b.Property<int?>("MaximumPreShiftOt");

                    b.Property<int?>("MinimumHolidayRestDayOt");

                    b.Property<int?>("MinimumPostShiftOt");

                    b.Property<int?>("MinimumPreShiftOt");

                    b.Property<DateTime?>("NightDiffEnd");

                    b.Property<DateTime?>("NightDiffStart");

                    b.Property<DateTime?>("PmbreakIn");

                    b.Property<DateTime?>("PmbreakOut");

                    b.Property<decimal?>("RequiredWorkHours");

                    b.Property<int?>("RoundHolidayRestDayOt");

                    b.Property<int?>("RoundPostShiftOt");

                    b.Property<int?>("RoundPreShiftOt");

                    b.Property<DateTime?>("ScheduleIn");

                    b.Property<DateTime?>("ScheduleOut");

                    b.Property<string>("ShiftCode");

                    b.Property<int?>("ShiftType");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Shift");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.User", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DPUserId");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.WorkSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("EmployeeId");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime>("ScheduleDate");

                    b.Property<Guid?>("ShiftId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("ShiftId");

                    b.ToTable("WorkSchedule");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.WorkSite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("IntegrationAuthPassword");

                    b.Property<string>("IntegrationAuthUsername");

                    b.Property<bool>("IsActive");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("WorkSite");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.AuditLog", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Calendar", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.CalendarDay", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.DailyTransactionRecord", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("TKProcessor.Models.TK.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftId");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Employee", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.EmployeeWorkSite", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.Employee", "Employee")
                        .WithMany("EmployeeWorkSites")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TKProcessor.Models.TK.WorkSite", "WorkSite")
                        .WithMany("WorkSiteEmployees")
                        .HasForeignKey("WorkSiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TKProcessor.Models.TK.ErrorLog", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.GlobalSetting", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Holiday", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Leave", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Mapping", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.GlobalSetting")
                        .WithMany("PayPackageMappings")
                        .HasForeignKey("GlobalSettingId");

                    b.HasOne("TKProcessor.Models.TK.GlobalSetting")
                        .WithMany("PayrollCodeMappings")
                        .HasForeignKey("GlobalSettingId1");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.RawData", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.SelectionSetting", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.GlobalSetting")
                        .WithMany("AutoApproveDTRFieldsList")
                        .HasForeignKey("GlobalSettingId");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Shift", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.User", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithOne("LastModifiedBy")
                        .HasForeignKey("TKProcessor.Models.TK.User", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TKProcessor.Models.TK.WorkSchedule", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("TKProcessor.Models.TK.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftId");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.WorkSite", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
                });
#pragma warning restore 612, 618
        }
    }
}
