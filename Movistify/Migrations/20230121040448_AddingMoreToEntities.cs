using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movistify.Migrations
{
    /// <inheritdoc />
    public partial class AddingMoreToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Year",
                table: "Movies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Actors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Actors");
        }
    }
}
