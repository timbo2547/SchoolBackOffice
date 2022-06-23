using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBackOffice.Infrastructure.Persistence.Migrations
{
    public partial class AddStudentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentUsers",
                columns: table => new
                {
                    StudentUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AspUserId = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    GradeLevelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentUsers", x => x.StudentUserId);
                    table.ForeignKey(
                        name: "FK_StudentUsers_AspNetUsers_AspUserId",
                        column: x => x.AspUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentUsers_GradeLevels_GradeLevelId",
                        column: x => x.GradeLevelId,
                        principalTable: "GradeLevels",
                        principalColumn: "GradeLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentUsers_AspUserId",
                table: "StudentUsers",
                column: "AspUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentUsers_GradeLevelId",
                table: "StudentUsers",
                column: "GradeLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentUsers");
        }
    }
}
