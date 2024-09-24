using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wot_api.Data.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedDate",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_RoleID",
                table: "Participants",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UsersId",
                table: "Participants",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Roles_RoleID",
                table: "Participants",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UsersId",
                table: "Participants",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Roles_RoleID",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UsersId",
                table: "Participants");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Participants_RoleID",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UsersId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "JoinedDate",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Participants");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Participants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
