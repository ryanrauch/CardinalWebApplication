using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardinalWebApplication.Data.Migrations
{
    public partial class _02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    ZoneID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    VisibleToLayersDelimited = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.ZoneID);
                });

            migrationBuilder.CreateTable(
                name: "ZoneShapes",
                columns: table => new
                {
                    ZoneShapeID = table.Column<Guid>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ParentZoneId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneShapes", x => x.ZoneShapeID);
                    table.ForeignKey(
                        name: "FK_ZoneShapes_Zones_ParentZoneId",
                        column: x => x.ParentZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZoneShapes_ParentZoneId",
                table: "ZoneShapes",
                column: "ParentZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZoneShapes");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
