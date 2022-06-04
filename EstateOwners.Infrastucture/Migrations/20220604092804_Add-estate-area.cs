using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class Addestatearea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Area",
                table: "Estates",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Estates");
        }
    }
}
