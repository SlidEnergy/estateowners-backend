using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class Addsigning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessagesToSign",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromChatId = table.Column<long>(nullable: false),
                    MessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesToSign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSignatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: true),
                    Messageid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSignatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estates_BuildingId",
                table: "Estates",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estates_Buildings_BuildingId",
                table: "Estates",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estates_Buildings_BuildingId",
                table: "Estates");

            migrationBuilder.DropTable(
                name: "MessagesToSign");

            migrationBuilder.DropTable(
                name: "UserSignatures");

            migrationBuilder.DropIndex(
                name: "IX_Estates_BuildingId",
                table: "Estates");
        }
    }
}
