using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBackOffice.Infrastructure.Persistence.Migrations
{
    public partial class AddEnrollmentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnrollmentStatusId",
                table: "StudentUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnrollmentStatus",
                columns: table => new
                {
                    EnrollmentStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentStatus", x => x.EnrollmentStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentUsers_EnrollmentStatusId",
                table: "StudentUsers",
                column: "EnrollmentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentUsers_EnrollmentStatus_EnrollmentStatusId",
                table: "StudentUsers",
                column: "EnrollmentStatusId",
                principalTable: "EnrollmentStatus",
                principalColumn: "EnrollmentStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentUsers_EnrollmentStatus_EnrollmentStatusId",
                table: "StudentUsers");

            migrationBuilder.DropTable(
                name: "EnrollmentStatus");

            migrationBuilder.DropIndex(
                name: "IX_StudentUsers_EnrollmentStatusId",
                table: "StudentUsers");

            migrationBuilder.DropColumn(
                name: "EnrollmentStatusId",
                table: "StudentUsers");
        }
    }
}
