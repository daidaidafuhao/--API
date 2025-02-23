﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI.Data;

#nullable disable

namespace WebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("WebAPI.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Education")
                        .HasColumnType("TEXT");

                    b.Property<string>("IDCardNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<string>("Level")
                        .HasColumnType("TEXT");

                    b.Property<string>("LevelJobType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Photo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RuzhiDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RuzhiDateEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RuzhiDateStart")
                        .HasColumnType("TEXT");

                    b.Property<string>("SchoolName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("UnitName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ZhuanYe")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IDCardNumber")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("WebAPI.Models.ImportHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IDCardNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<int>("ImportCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ImportTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IDCardNumber");

                    b.ToTable("ImportHistories");
                });

            modelBuilder.Entity("WebAPI.Models.PhotoTable", b =>
                {
                    b.Property<string>("IDCardNumber")
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("IDCardNumber");

                    b.ToTable("PhotoTables");
                });

            modelBuilder.Entity("WebAPI.Models.TrainingRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Assessment")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remarks")
                        .HasColumnType("TEXT");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TrainingContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TrainingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("TrainingLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TrainingUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("TrainingRecords");
                });

            modelBuilder.Entity("WebAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebAPI.Models.ImportHistory", b =>
                {
                    b.HasOne("WebAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("IDCardNumber")
                        .HasPrincipalKey("IDCardNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("WebAPI.Models.PhotoTable", b =>
                {
                    b.HasOne("WebAPI.Models.Employee", null)
                        .WithOne()
                        .HasForeignKey("WebAPI.Models.PhotoTable", "IDCardNumber")
                        .HasPrincipalKey("WebAPI.Models.Employee", "IDCardNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebAPI.Models.TrainingRecord", b =>
                {
                    b.HasOne("WebAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}
