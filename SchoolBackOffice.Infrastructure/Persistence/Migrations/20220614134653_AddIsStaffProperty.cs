using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBackOffice.Infrastructure.Persistence.Migrations
{
    public partial class AddIsStaffProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStaff",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStaff",
                table: "AspNetUsers");
        }
    }
}
