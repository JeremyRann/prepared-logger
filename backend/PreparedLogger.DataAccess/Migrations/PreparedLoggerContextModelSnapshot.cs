﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PreparedLogger.DataAccess;

namespace PreparedLogger.DataAccess.Migrations
{
    [DbContext(typeof(PreparedLoggerContext))]
    partial class PreparedLoggerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PreparedLogger.Models.Log", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.HasKey("LogID");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("PreparedLogger.Models.LogEntry", b =>
                {
                    b.Property<int>("LogEntryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LogID");

                    b.Property<string>("Message");

                    b.Property<DateTime>("Time");

                    b.HasKey("LogEntryID");

                    b.HasIndex("LogID");

                    b.ToTable("LogEntry");
                });

            modelBuilder.Entity("PreparedLogger.Models.LogEntry", b =>
                {
                    b.HasOne("PreparedLogger.Models.Log", "Log")
                        .WithMany("LogEntries")
                        .HasForeignKey("LogID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
