﻿// <auto-generated />
using System;
using Microservice.Email.Infrastructure.Persistence;
using Microservice.Email.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservice.Email.Migrations
{
    [DbContext(typeof(ApplicationDatabaseContext))]
    [Migration("20230708235452_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microservice.Email.Domain.Entities.EmailEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<int>("EmailStatus")
                        .HasColumnType("integer")
                        .HasColumnName("email_status");

                    b.Property<string>("Recipients")
                        .IsRequired()
                        .HasMaxLength(8000)
                        .HasColumnType("character varying(8000)")
                        .HasColumnName("recipients");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sender");

                    b.Property<DateTimeOffset>("SentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_date");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("subject");

                    b.HasKey("Id")
                        .HasName("pk_email");

                    b.ToTable("Email", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
