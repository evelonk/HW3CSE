using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileAnalysisService.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentAndStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignmentName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedAt",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CheckedFileId",
                table: "Reports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPlagiarism",
                table: "Reports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchedFileId",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxSimilarityPercent",
                table: "Reports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CheckedAt",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CheckedFileId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsPlagiarism",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MatchedFileId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MaxSimilarityPercent",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Reports");
        }
    }
}
