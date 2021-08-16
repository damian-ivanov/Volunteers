using Microsoft.EntityFrameworkCore.Migrations;

namespace Volunteers.data.migrations
{
    public partial class IntroducingProjectCoordinatesAsAnObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
