using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class adduniquekeytocandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId_Type",
                table: "Candidates",
                columns: new[] { "UserId", "Type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Candidates_UserId_Type",
                table: "Candidates");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                column: "UserId");
        }
    }
}
