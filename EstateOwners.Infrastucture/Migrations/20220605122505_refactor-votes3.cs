using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateOwners.Infrastucture.Migrations
{
    public partial class refactorvotes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSignatures",
                table: "UserSignatures");

            migrationBuilder.RenameTable(
                name: "VotesForCandidates",
                newName: "CandidateVotes");

            migrationBuilder.RenameTable(
                name: "UserSignatures",
                newName: "UserMessageSignatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateVotes",
                table: "CandidateVotes",
                columns: new[] { "UserId", "CandidateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessageSignatures",
                table: "UserMessageSignatures",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessageSignatures",
                table: "UserMessageSignatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateVotes",
                table: "CandidateVotes");

            migrationBuilder.RenameTable(
                name: "UserMessageSignatures",
                newName: "UserSignatures");

            migrationBuilder.RenameTable(
                name: "CandidateVotes",
                newName: "VotesForCandidates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSignatures",
                table: "UserSignatures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotesForCandidates",
                table: "VotesForCandidates",
                columns: new[] { "UserId", "CandidateId" });
        }
    }
}
