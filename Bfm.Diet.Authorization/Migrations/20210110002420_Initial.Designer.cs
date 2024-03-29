﻿// <auto-generated />
using System;
using Bfm.Diet.Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bfm.Diet.Authorization.Migrations
{
    [DbContext(typeof(AuthorizationDbContext))]
    [Migration("20210110002420_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Bfm.Diet.Authorization.Model.Grup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Description");

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<DateTime?>("GuncellemeTarihi")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Guncelleyen")
                        .HasColumnType("integer");

                    b.Property<int?>("Kaydeden")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("KayitTarihi")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("grup", "diet_auth_data");
                });

            modelBuilder.Entity("Bfm.Diet.Authorization.Model.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("adi");

                    b.Property<bool>("Durum")
                        .HasColumnType("boolean")
                        .HasColumnName("durum");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime?>("GuncellemeTarihi")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("guncelleme_tarihi");

                    b.Property<int?>("Guncelleyen")
                        .HasColumnType("integer")
                        .HasColumnName("guncelleyen");

                    b.Property<int?>("Kaydeden")
                        .HasColumnType("integer")
                        .HasColumnName("kaydeden");

                    b.Property<DateTime?>("KayitTarihi")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("kayit_tarihi");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("passwordHash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("passwordSalt");

                    b.Property<string>("Soyadi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("soyadi");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("kullanici", "diet_auth_data");
                });
#pragma warning restore 612, 618
        }
    }
}
