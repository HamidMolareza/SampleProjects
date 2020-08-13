using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleForm_RegisterUserWithPhoto.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 35, nullable: false),
                    Family = table.Column<string>(maxLength: 35, nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Agreement = table.Column<bool>(nullable: false),
                    RegisterDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
