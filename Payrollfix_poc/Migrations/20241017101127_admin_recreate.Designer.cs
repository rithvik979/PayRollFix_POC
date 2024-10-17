﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payrollfix_poc.Data;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    [DbContext(typeof(PayRollFix_pocContext))]
    [Migration("20241017101127_admin_recreate")]
    partial class admin_recreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Payrollfix_poc.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminId");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Attandence", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceId"));

                    b.Property<DateTime?>("CheckInTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CheckOutTime")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("WorkHours")
                        .HasColumnType("real");

                    b.HasKey("AttendanceId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<string>("DeptDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("date");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId1")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("JoinDate")
                        .HasColumnType("date");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone_no")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("PositionId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Expense", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("ExpenseType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExpenseId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Leave", b =>
                {
                    b.Property<int>("LeaveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaveId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LeaveType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LeaveId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.LeaveBalance", b =>
                {
                    b.Property<int>("BalanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BalanceId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("MaxDays")
                        .HasColumnType("int");

                    b.Property<int>("UsedDays")
                        .HasColumnType("int");

                    b.HasKey("BalanceId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("LeaveBalances");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.LoginActivity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActivityId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ActivityId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("LoginActivities");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.LoginViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RememberMe")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("LoginViewModel");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionId"));

                    b.Property<string>("PositionDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PositionId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Timesheet", b =>
                {
                    b.Property<int>("TimesheetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<decimal>("HoursWorked")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TimesheetId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Attandence", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", "Employee")
                        .WithMany("Attandences")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Employee", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payrollfix_poc.Models.Employee", "employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payrollfix_poc.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.Navigation("Department");

                    b.Navigation("Position");

                    b.Navigation("employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Expense", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", null)
                        .WithMany("Expenses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Leave", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", "Employee")
                        .WithMany("Leaves")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.LeaveBalance", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", "Employee")
                        .WithMany("LeaveBalances")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.LoginActivity", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", "Employee")
                        .WithMany("LoginActivities")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Timesheet", b =>
                {
                    b.HasOne("Payrollfix_poc.Models.Employee", null)
                        .WithMany("Timesheets")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Payrollfix_poc.Models.Employee", b =>
                {
                    b.Navigation("Attandences");

                    b.Navigation("Expenses");

                    b.Navigation("LeaveBalances");

                    b.Navigation("Leaves");

                    b.Navigation("LoginActivities");

                    b.Navigation("Timesheets");
                });
#pragma warning restore 612, 618
        }
    }
}
