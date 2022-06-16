using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBackOffice.Infrastructure.Persistence.Migrations
{
    public partial class UpdateStaffUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffUser_AspNetUsers_AspUserId",
                table: "StaffUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffUser",
                table: "StaffUser");

            migrationBuilder.RenameTable(
                name: "StaffUser",
                newName: "StaffUsers");

            migrationBuilder.RenameIndex(
                name: "IX_StaffUser_AspUserId",
                table: "StaffUsers",
                newName: "IX_StaffUsers_AspUserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "StaffUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StaffUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffUsers",
                table: "StaffUsers",
                column: "StaffUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffUsers_AspNetUsers_AspUserId",
                table: "StaffUsers",
                column: "AspUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffUsers_AspNetUsers_AspUserId",
                table: "StaffUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffUsers",
                table: "StaffUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "StaffUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StaffUsers");

            migrationBuilder.RenameTable(
                name: "StaffUsers",
                newName: "StaffUser");

            migrationBuilder.RenameIndex(
                name: "IX_StaffUsers_AspUserId",
                table: "StaffUser",
                newName: "IX_StaffUser_AspUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffUser",
                table: "StaffUser",
                column: "StaffUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffUser_AspNetUsers_AspUserId",
                table: "StaffUser",
                column: "AspUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
