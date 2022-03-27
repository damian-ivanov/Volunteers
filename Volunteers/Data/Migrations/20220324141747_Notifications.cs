using Microsoft.EntityFrameworkCore.Migrations;

namespace Volunteers.data.migrations
{
    public partial class Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NotificationId",
                table: "AspNetUsers",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Notifications_NotificationId",
                table: "AspNetUsers",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Notifications_NotificationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NotificationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "AspNetUsers");
        }
    }
}
