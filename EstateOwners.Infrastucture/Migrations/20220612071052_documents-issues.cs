using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class documentsissues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesToSign");

            migrationBuilder.DropTable(
                name: "UserMessageSignatures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "AuthTokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateTable(
                name: "DocumentTelegramMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromChatId = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTelegramMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueTelegramMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromChatId = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTelegramMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMessageVotes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    VoteTelegramMessageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageVotes", x => new { x.UserId, x.VoteTelegramMessageId });
                });

            migrationBuilder.CreateTable(
                name: "VoteTelegramMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromChatId = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteTelegramMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTelegramMessages");

            migrationBuilder.DropTable(
                name: "IssueTelegramMessages");

            migrationBuilder.DropTable(
                name: "UserMessageVotes");

            migrationBuilder.DropTable(
                name: "VoteTelegramMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "AuthTokens",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "MessagesToSign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromChatId = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesToSign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMessageSignatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Messageid = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageSignatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageSignatures_UserId_Messageid",
                table: "UserMessageSignatures",
                columns: new[] { "UserId", "Messageid" },
                unique: true);
        }
    }
}
