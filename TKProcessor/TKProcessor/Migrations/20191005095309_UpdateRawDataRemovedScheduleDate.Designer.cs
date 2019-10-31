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
    [Migration("20191005095309_UpdateRawDataRemovedScheduleDate")]
    partial class UpdateRawDataRemovedScheduleDate
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

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("NewValue");

                    b.Property<string>("OldValue");

                    b.Property<string>("Target");

                    b.HasKey("Id");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Calendar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Calendar");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.DailyTransactionRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("AbsentHours");

                    b.Property<decimal?>("ActualLate");

                    b.Property<decimal?>("ActualLegHol");

                    b.Property<decimal?>("ActualLegHolOt");

                    b.Property<decimal?>("ActualLegHolRd");

                    b.Property<decimal?>("ActualLegHolRdot");

                    b.Property<decimal?>("ActualLegNdholOt");

                    b.Property<decimal?>("ActualLegNdholRdot");

                    b.Property<decimal?>("ActualLegSpeHol");

                    b.Property<decimal?>("ActualLegSpeHolOt");

                    b.Property<decimal?>("ActualLegSpeHolRd");

                    b.Property<decimal?>("ActualLegSpeHolRdot");

                    b.Property<decimal?>("ActualNdlegHol");

                    b.Property<decimal?>("ActualNdlegHolRd");

                    b.Property<decimal?>("ActualNdlegSpeHol");

                    b.Property<decimal?>("ActualNdlegSpeHolOt");

                    b.Property<decimal?>("ActualNdlegSpeHolRd");

                    b.Property<decimal?>("ActualNdlegSpeHolRdot");

                    b.Property<decimal?>("ActualNdrd");

                    b.Property<decimal?>("ActualNdrdot");

                    b.Property<decimal?>("ActualNdspeHol");

                    b.Property<decimal?>("ActualNdspeHolOt");

                    b.Property<decimal?>("ActualNdspeHolRd");

                    b.Property<decimal?>("ActualNdspeHolRdot");

                    b.Property<decimal?>("ActualOvertime");

                    b.Property<decimal?>("ActualPostOvertime");

                    b.Property<decimal?>("ActualPreOvertime");

                    b.Property<decimal>("ActualRestDay");

                    b.Property<decimal?>("ActualRestDayOt");

                    b.Property<decimal?>("ActualSpeHol");

                    b.Property<decimal?>("ActualSpeHolOt");

                    b.Property<decimal?>("ActualSpeHolRd");

                    b.Property<decimal?>("ActualSpeHolRdot");

                    b.Property<decimal?>("ActualUndertime");

                    b.Property<decimal?>("Allowance");

                    b.Property<decimal?>("ApprovedLate");

                    b.Property<decimal?>("ApprovedLegHol");

                    b.Property<decimal?>("ApprovedLegHolOt");

                    b.Property<decimal?>("ApprovedLegHolRd");

                    b.Property<decimal?>("ApprovedLegHolRdot");

                    b.Property<decimal?>("ApprovedLegSpeHol");

                    b.Property<decimal?>("ApprovedLegSpeHolOt");

                    b.Property<decimal?>("ApprovedLegSpeHolRd");

                    b.Property<decimal?>("ApprovedLegSpeHolRdot");

                    b.Property<decimal?>("ApprovedNdlegHol");

                    b.Property<decimal?>("ApprovedNdlegHolOt");

                    b.Property<decimal?>("ApprovedNdlegHolRd");

                    b.Property<decimal?>("ApprovedNdlegHolRdot");

                    b.Property<decimal?>("ApprovedNdlegSpeHol");

                    b.Property<decimal?>("ApprovedNdlegSpeHolOt");

                    b.Property<decimal?>("ApprovedNdlegSpeHolRd");

                    b.Property<decimal?>("ApprovedNdlegSpeHolRdot");

                    b.Property<decimal?>("ApprovedNdrd");

                    b.Property<decimal?>("ApprovedNdrdot");

                    b.Property<decimal?>("ApprovedNdspeHol");

                    b.Property<decimal?>("ApprovedNdspeHolOt");

                    b.Property<decimal?>("ApprovedNdspeHolRd");

                    b.Property<decimal?>("ApprovedNdspeHolRdot");

                    b.Property<decimal?>("ApprovedOvertime");

                    b.Property<decimal?>("ApprovedPostOvertime");

                    b.Property<decimal?>("ApprovedPreOvertime");

                    b.Property<decimal?>("ApprovedRestDay");

                    b.Property<decimal?>("ApprovedRestDayOt");

                    b.Property<decimal?>("ApprovedSpeHol");

                    b.Property<decimal?>("ApprovedSpeHolOt");

                    b.Property<decimal?>("ApprovedSpeHolRd");

                    b.Property<decimal?>("ApprovedSpeHolRdot");

                    b.Property<decimal?>("ApprovedUndertime");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("EmployeeCode");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("LeaveType");

                    b.Property<string>("ModifiedIo");

                    b.Property<decimal?>("NightDifferential");

                    b.Property<decimal?>("NightDifferentialOt");

                    b.Property<string>("Remarks");

                    b.Property<string>("ShiftCode");

                    b.Property<DateTime?>("TimeIn");

                    b.Property<DateTime?>("TimeOut");

                    b.Property<DateTime?>("TransactionDate");

                    b.Property<decimal?>("WorkHours");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

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

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Password");

                    b.Property<DateTime?>("TerminationDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.ErrorLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateRaised");

                    b.Property<string>("Message");

                    b.Property<string>("Source");

                    b.Property<string>("StackTrace");

                    b.HasKey("Id");

                    b.ToTable("ErrorLog");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.GlobalSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("GlobalSetting");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Holiday", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CalendarId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LastModifiedById");

                    b.ToTable("Holiday");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.Leave", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmployeeCode");

                    b.Property<DateTime>("LeaveDate");

                    b.Property<decimal>("LeaveHours");

                    b.Property<string>("LeaveType");

                    b.HasKey("Id");

                    b.ToTable("Leave");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.RawData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BiometricsId");

                    b.Property<DateTime?>("TransactionDateTime");

                    b.Property<int?>("TransactionType");

                    b.HasKey("Id");

                    b.ToTable("RawData");
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

                    b.Property<int?>("FocusDate");

                    b.Property<int?>("GracePeriodEarlyOut");

                    b.Property<int?>("GracePeriodLateIn");

                    b.Property<bool?>("IsEarlyOut");

                    b.Property<bool?>("IsHolidayRestDayOt");

                    b.Property<bool?>("IsLateIn");

                    b.Property<bool?>("IsPlusEarlyOutMinutes");

                    b.Property<bool?>("IsPlusLateInMinutes");

                    b.Property<bool?>("IsPostShiftOt");

                    b.Property<bool?>("IsPreShiftOt");

                    b.Property<bool?>("IsRestDay");

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.Property<Guid?>("LastModifiedById");

                    b.Property<DateTime?>("LastModifiedOn");

                    b.Property<DateTime>("ScheduleDate");

                    b.Property<Guid?>("ShiftCodeId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("ShiftCodeId");

                    b.ToTable("WorkSchedule");
                });

            modelBuilder.Entity("TKProcessor.Models.TK.DailyTransactionRecord", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
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

            modelBuilder.Entity("TKProcessor.Models.TK.Holiday", b =>
                {
                    b.HasOne("TKProcessor.Models.TK.Calendar")
                        .WithMany("Holidays")
                        .HasForeignKey("CalendarId");

                    b.HasOne("TKProcessor.Models.TK.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TKProcessor.Models.TK.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");
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

                    b.HasOne("TKProcessor.Models.TK.Shift", "ShiftCode")
                        .WithMany()
                        .HasForeignKey("ShiftCodeId");
                });
#pragma warning restore 612, 618
        }
    }
}
