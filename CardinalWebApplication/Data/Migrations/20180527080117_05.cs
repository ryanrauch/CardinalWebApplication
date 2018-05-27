using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardinalWebApplication.Data.Migrations
{
    public partial class _05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendGroupUsers_FriendGroups_GroupID1",
                table: "FriendGroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendGroupUsers_GroupID1",
                table: "FriendGroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupID1",
                table: "FriendGroupUsers");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "FriendGroupUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupID",
                table: "FriendGroupUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendID",
                table: "FriendGroupUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "FriendGroups",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendGroupUsers_FriendID",
                table: "FriendGroupUsers",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendGroupUsers_GroupID",
                table: "FriendGroupUsers",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendGroups_UserID",
                table: "FriendGroups",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendGroups_AspNetUsers_UserID",
                table: "FriendGroups",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendGroupUsers_AspNetUsers_FriendID",
                table: "FriendGroupUsers",
                column: "FriendID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendGroupUsers_FriendGroups_GroupID",
                table: "FriendGroupUsers",
                column: "GroupID",
                principalTable: "FriendGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendGroups_AspNetUsers_UserID",
                table: "FriendGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendGroupUsers_AspNetUsers_FriendID",
                table: "FriendGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendGroupUsers_FriendGroups_GroupID",
                table: "FriendGroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendGroupUsers_FriendID",
                table: "FriendGroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendGroupUsers_GroupID",
                table: "FriendGroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendGroups_UserID",
                table: "FriendGroups");

            migrationBuilder.DropColumn(
                name: "FriendID",
                table: "FriendGroupUsers");

            migrationBuilder.AlterColumn<string>(
                name: "GroupID",
                table: "FriendGroupUsers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "GroupID1",
                table: "FriendGroupUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerID",
                table: "FriendGroupUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "FriendGroups",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendGroupUsers_GroupID1",
                table: "FriendGroupUsers",
                column: "GroupID1");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendGroupUsers_FriendGroups_GroupID1",
                table: "FriendGroupUsers",
                column: "GroupID1",
                principalTable: "FriendGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
