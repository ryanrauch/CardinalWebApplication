using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardinalWebApplication.Data.Migrations
{
    public partial class _03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ARGBFill",
                table: "Zones",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Zones",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ARGBFill",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Zones");
        }
    }
}
