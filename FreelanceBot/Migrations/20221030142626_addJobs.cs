using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceBot.Migrations
{
    public partial class addJobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    PayMin = table.Column<int>(nullable: false),
                    PayMax = table.Column<int>(nullable: false),
                    Pay = table.Column<int>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    TypeJob = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
