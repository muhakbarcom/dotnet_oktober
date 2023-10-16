using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_oktober.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HitPoints = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CREATED_DT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHANGED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CHANGED_DT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
