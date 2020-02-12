using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SawMill.Data.Migrations
{
    public partial class RemoveCorrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Correlation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Correlation",
                columns: table => new
                {
                    CorrelationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CauseValue = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    EffectValue = table.Column<string>(nullable: true),
                    IsUserCorrelation = table.Column<bool>(nullable: false),
                    SystemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Correlation", x => x.CorrelationId);
                    table.ForeignKey(
                        name: "FK_Correlation_System",
                        column: x => x.SystemId,
                        principalTable: "System",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Correlation_SystemId",
                table: "Correlation",
                column: "SystemId");
        }
    }
}
