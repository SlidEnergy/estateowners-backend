using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class adduniqueindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserMessageSignatures_UserId_Messageid",
                table: "UserMessageSignatures",
                columns: new[] { "UserId", "Messageid" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserMessageSignatures_UserId_Messageid",
                table: "UserMessageSignatures");
        }
    }
}
