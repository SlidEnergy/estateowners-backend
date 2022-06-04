using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class removetelegramuserfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanJoinGroups",
                table: "TelegramUsers");

            migrationBuilder.DropColumn(
                name: "CanReadAllGroupMessages",
                table: "TelegramUsers");

            migrationBuilder.DropColumn(
                name: "SupportsInlineQueries",
                table: "TelegramUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanJoinGroups",
                table: "TelegramUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanReadAllGroupMessages",
                table: "TelegramUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsInlineQueries",
                table: "TelegramUsers",
                type: "boolean",
                nullable: true);
        }
    }
}
