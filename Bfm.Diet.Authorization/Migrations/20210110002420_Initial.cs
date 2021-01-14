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
                name: "diet_auth_data");

            migrationBuilder.CreateTable(
                name: "grup",
                schema: "diet_auth_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Kaydeden = table.Column<int>(type: "integer", nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Guncelleyen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kullanici",
                schema: "diet_auth_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adi = table.Column<string>(type: "text", nullable: false),
                    soyadi = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false),
                    kayit_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kaydeden = table.Column<int>(type: "integer", nullable: true),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    guncelleyen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanici", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kullanici_email",
                schema: "diet_auth_data",
                table: "kullanici",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grup",
                schema: "diet_auth_data");

            migrationBuilder.DropTable(
                name: "kullanici",
                schema: "diet_auth_data");
        }
    }
}
