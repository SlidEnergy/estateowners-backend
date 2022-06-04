using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class Removeidfromvote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VotesForCandidates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VotesForCandidates",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates",
                columns: new[] { "UserId", "CandidateId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VotesForCandidates",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VotesForCandidates",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates",
                column: "Id");
        }
    }
}
