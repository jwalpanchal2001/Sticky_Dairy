﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sticky_Dairy.Infrastructure.Data;

#nullable disable

namespace Sticky_Dairy.Infrastructure.Migrations
{
    [DbContext(typeof(Sticky_Dairy_dbContext))]
    [Migration("20250211050540_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Attachment", b =>
                {
                    b.Property<Guid>("AttachmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("NoteID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("AttachmentID");

                    b.HasIndex("NoteID");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Note", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteId");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Reminder", b =>
                {
                    b.Property<Guid>("ReminderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("NoteID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReminderAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ReminderID");

                    b.HasIndex("NoteID");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Attachment", b =>
                {
                    b.HasOne("Sticky_Dairy.Domain.Models.Entities.Note", "Note")
                        .WithMany("Attachments")
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Note", b =>
                {
                    b.HasOne("Sticky_Dairy.Domain.Models.Entities.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Reminder", b =>
                {
                    b.HasOne("Sticky_Dairy.Domain.Models.Entities.Note", "Note")
                        .WithMany("Reminders")
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.Note", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Reminders");
                });

            modelBuilder.Entity("Sticky_Dairy.Domain.Models.Entities.User", b =>
                {
                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
