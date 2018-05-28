using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardinalWebApplication.Data.Migrations
{
    public partial class _06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentZoneId",
                table: "CurrentLayers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentZoneZoneID",
                table: "CurrentLayers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrentLayers_CurrentZoneZoneID",
                table: "CurrentLayers",
                column: "CurrentZoneZoneID");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentLayers_Zones_CurrentZoneZoneID",
                table: "CurrentLayers",
                column: "CurrentZoneZoneID",
                principalTable: "Zones",
                principalColumn: "ZoneID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentLayers_Zones_CurrentZoneZoneID",
                table: "CurrentLayers");

            migrationBuilder.DropIndex(
                name: "IX_CurrentLayers_CurrentZoneZoneID",
                table: "CurrentLayers");

            migrationBuilder.DropColumn(
                name: "CurrentZoneId",
                table: "CurrentLayers");

            migrationBuilder.DropColumn(
                name: "CurrentZoneZoneID",
                table: "CurrentLayers");
        }
    }
}
