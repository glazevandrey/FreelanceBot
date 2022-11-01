using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceBot.Migrations
{
    public partial class AddMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxEvents",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxJobs",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxResumes",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxEvents",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaxJobs",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaxResumes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Events");
        }
    }
}
