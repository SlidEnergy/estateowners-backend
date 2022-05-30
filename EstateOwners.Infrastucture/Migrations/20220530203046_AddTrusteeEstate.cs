using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class AddTrusteeEstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrusteeEstates",
                columns: table => new
                {
                    TrusteeId = table.Column<int>(nullable: false),
                    EstateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrusteeEstates", x => new { x.EstateId, x.TrusteeId });
                    table.ForeignKey(
                        name: "FK_TrusteeEstates_Estates_EstateId",
                        column: x => x.EstateId,
                        principalTable: "Estates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrusteeEstates_Trustee_TrusteeId",
                        column: x => x.TrusteeId,
                        principalTable: "Trustee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrusteeEstates_TrusteeId",
                table: "TrusteeEstates",
                column: "TrusteeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrusteeEstates");
        }
    }
}
