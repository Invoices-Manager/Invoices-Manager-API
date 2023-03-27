﻿// <auto-generated />
using System;
using Invoices_Manager_API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Invoices_Manager_API.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Invoices_Manager_API.Models.BackUpInfoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BackUpId")
                        .HasColumnType("int");

                    b.Property<string>("BackUpName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("BackUpSize")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EntityCountInvoices")
                        .HasColumnType("int");

                    b.Property<int>("EntityCountNotes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BackUpId");

                    b.ToTable("BackUpInfo");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.BackUpModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("BackUp");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.InvoiceBackUpModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BackUpModelId")
                        .HasColumnType("int");

                    b.Property<string>("Base64")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BackUpModelId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceBackUp");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.InvoiceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CaptureDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExhibitionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FileID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ImportanceState")
                        .HasColumnType("int");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("longtext");

                    b.Property<int>("MoneyState")
                        .HasColumnType("int");

                    b.Property<double>("MoneyTotal")
                        .HasColumnType("double");

                    b.Property<string>("Organization")
                        .HasColumnType("longtext");

                    b.Property<int>("PaidState")
                        .HasColumnType("int");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext");

                    b.Property<string>("TagsAsString")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Tags");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.LoginModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LoginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.NoteModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.BackUpInfoModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.BackUpModel", "BackUp")
                        .WithMany()
                        .HasForeignKey("BackUpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BackUp");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.BackUpModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.UserModel", null)
                        .WithMany("BackUps")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.InvoiceBackUpModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.BackUpModel", null)
                        .WithMany("Invoices")
                        .HasForeignKey("BackUpModelId");

                    b.HasOne("Invoices_Manager_API.Models.InvoiceModel", "Invoice")
                        .WithMany()
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.InvoiceModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.UserModel", null)
                        .WithMany("Invoices")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.LoginModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.UserModel", null)
                        .WithMany("Logins")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.NoteModel", b =>
                {
                    b.HasOne("Invoices_Manager_API.Models.UserModel", null)
                        .WithMany("Notebook")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.BackUpModel", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("Invoices_Manager_API.Models.UserModel", b =>
                {
                    b.Navigation("BackUps");

                    b.Navigation("Invoices");

                    b.Navigation("Logins");

                    b.Navigation("Notebook");
                });
#pragma warning restore 612, 618
        }
    }
}
