﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bfm.Diet.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "diet_app_data");

            migrationBuilder.CreateTable(
                name: "sabit_tanim",
                schema: "diet_app_data",
                columns: table => new
                {
                    sabit_tanim_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Adi = table.Column<string>(type: "text", nullable: true),
                    aciklama = table.Column<string>(type: "text", nullable: false),
                    kayit_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kaydeden = table.Column<int>(type: "integer", nullable: true),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    guncelleyen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sabit_tanim", x => x.sabit_tanim_id);
                });

            migrationBuilder.CreateTable(
                name: "sabit_tanim_detay",
                schema: "diet_app_data",
                columns: table => new
                {
                    sabit_tanim_detay_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sabit_tanim_id = table.Column<int>(type: "integer", nullable: false),
                    kodu = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    aciklamasi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ozel_kod = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    kayit_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kaydeden = table.Column<int>(type: "integer", nullable: true),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    guncelleyen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sabit_tanim_detay", x => x.sabit_tanim_detay_id);
                    table.ForeignKey(
                        name: "FK_sabit_tanim_detay_sabit_tanim_sabit_tanim_id",
                        column: x => x.sabit_tanim_id,
                        principalSchema: "diet_app_data",
                        principalTable: "sabit_tanim",
                        principalColumn: "sabit_tanim_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sabit_tanim_detay_sabit_tanim_id",
                schema: "diet_app_data",
                table: "sabit_tanim_detay",
                column: "sabit_tanim_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sabit_tanim_detay",
                schema: "diet_app_data");

            migrationBuilder.DropTable(
                name: "sabit_tanim",
                schema: "diet_app_data");
        }
    }
}
