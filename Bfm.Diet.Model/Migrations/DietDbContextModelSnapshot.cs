﻿// <auto-generated />
using System;
using Bfm.Diet.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bfm.Diet.Model.Migrations
{
    [DbContext(typeof(DietDbContext))]
    partial class DietDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Bfm.Diet.Model.SabitTanim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sabit_tanim_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("aciklama");

                    b.Property<string>("Adi")
                        .HasColumnType("text");

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

                    b.HasKey("Id");

                    b.ToTable("sabit_tanim", "diet_app_data");
                });

            modelBuilder.Entity("Bfm.Diet.Model.SabitTanimDetay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sabit_tanim_detay_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Aciklamasi")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("aciklamasi");

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

                    b.Property<string>("Kodu")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("kodu");

                    b.Property<string>("OzelKod")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("ozel_kod");

                    b.Property<int>("SabitTanimId")
                        .HasColumnType("integer")
                        .HasColumnName("sabit_tanim_id");

                    b.HasKey("Id");

                    b.HasIndex("SabitTanimId");

                    b.ToTable("sabit_tanim_detay", "diet_app_data");
                });

            modelBuilder.Entity("Bfm.Diet.Model.SabitTanimDetay", b =>
                {
                    b.HasOne("Bfm.Diet.Model.SabitTanim", "SabitTanim")
                        .WithMany("SabitTanimDetaylari")
                        .HasForeignKey("SabitTanimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SabitTanim");
                });

            modelBuilder.Entity("Bfm.Diet.Model.SabitTanim", b =>
                {
                    b.Navigation("SabitTanimDetaylari");
                });
#pragma warning restore 612, 618
        }
    }
}
