using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bfm.Diet.Authorization.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "diet_auth_data");

            migrationBuilder.CreateTable(
                "grup",
                schema: "diet_auth_data",
                columns: table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: false),
                    Description = table.Column<string>("text", nullable: false),
                    KayitTarihi = table.Column<DateTime>("timestamp without time zone", nullable: true),
                    Kaydeden = table.Column<int>("integer", nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>("timestamp without time zone", nullable: true),
                    Guncelleyen = table.Column<int>("integer", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_grup", x => x.Id); });

            migrationBuilder.CreateTable(
                "kullanici",
                schema: "diet_auth_data",
                columns: table => new
                {
                    id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adi = table.Column<string>("text", nullable: false),
                    soyadi = table.Column<string>("text", nullable: false),
                    email = table.Column<string>("text", nullable: false),
                    passwordSalt = table.Column<byte[]>("bytea", nullable: false),
                    passwordHash = table.Column<byte[]>("bytea", nullable: false),
                    durum = table.Column<bool>("boolean", nullable: false),
                    kayit_tarihi = table.Column<DateTime>("timestamp without time zone", nullable: true),
                    kaydeden = table.Column<int>("integer", nullable: true),
                    guncelleme_tarihi = table.Column<DateTime>("timestamp without time zone", nullable: true),
                    guncelleyen = table.Column<int>("integer", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_kullanici", x => x.id); });

            migrationBuilder.CreateIndex(
                "IX_kullanici_email",
                schema: "diet_auth_data",
                table: "kullanici",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "grup",
                "diet_auth_data");

            migrationBuilder.DropTable(
                "kullanici",
                "diet_auth_data");
        }
    }
}