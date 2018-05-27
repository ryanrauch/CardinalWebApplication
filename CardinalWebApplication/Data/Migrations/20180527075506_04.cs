using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardinalWebApplication.Data.Migrations
{
    public partial class _04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FriendGroupUsers",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    GroupID = table.Column<string>(nullable: true),
                    GroupID1 = table.Column<Guid>(nullable: true),
                    OwnerID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendGroupUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FriendGroupUsers_FriendGroups_GroupID1",
                        column: x => x.GroupID1,
                        principalTable: "FriendGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendGroupUsers_GroupID1",
                table: "FriendGroupUsers",
                column: "GroupID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendGroupUsers");

            migrationBuilder.DropTable(
                name: "FriendGroups");
        }
    }
}
