using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class Addbase64signature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Base64Image",
                table: "UserSignatures",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64Image",
                table: "UserSignatures");
        }
    }
}
