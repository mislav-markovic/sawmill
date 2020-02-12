using Microsoft.EntityFrameworkCore.Migrations;

namespace SawMill.Data.Migrations
{
    public partial class AlertGroupALert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_AlertGroup",
                table: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Alert_AlertGroupId",
                table: "Alert");

            migrationBuilder.DropColumn(
                name: "AlertGroupId",
                table: "Alert");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Alert");

            migrationBuilder.CreateTable(
                name: "AlertGroupAlert",
                columns: table => new
                {
                    AlertGroupId = table.Column<int>(nullable: false),
                    AlertId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Not = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertGroupAlert", x => new { x.AlertId, x.AlertGroupId });
                    table.ForeignKey(
                        name: "FK_AlertGroup_AlertGroupAlert",
                        column: x => x.AlertGroupId,
                        principalTable: "AlertGroup",
                        principalColumn: "AlertGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alert_AlertGroupAlert",
                        column: x => x.AlertId,
                        principalTable: "Alert",
                        principalColumn: "AlertId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertGroupAlert_AlertGroupId",
                table: "AlertGroupAlert",
                column: "AlertGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertGroupAlert");

            migrationBuilder.AddColumn<int>(
                name: "AlertGroupId",
                table: "Alert",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Alert",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertGroupId",
                table: "Alert",
                column: "AlertGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_AlertGroup",
                table: "Alert",
                column: "AlertGroupId",
                principalTable: "AlertGroup",
                principalColumn: "AlertGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
