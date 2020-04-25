using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class DeletedImportantDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_Notes_NoteId",
                table: "Examinations");

            migrationBuilder.DropTable(
                name: "ImportantDatePets");

            migrationBuilder.DropTable(
                name: "ImportantDates");

            migrationBuilder.DropIndex(
                name: "IX_Examinations_NoteId",
                table: "Examinations");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Examinations");

            migrationBuilder.AddColumn<DateTime>(
                name: "ImportantDate",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Examinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportantDate",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Examinations");

            migrationBuilder.AddColumn<Guid>(
                name: "NoteId",
                table: "Examinations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImportantDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportantDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportantDates_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportantDatePets",
                columns: table => new
                {
                    ImportantDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportantDatePets", x => new { x.ImportantDateId, x.PetId });
                    table.ForeignKey(
                        name: "FK_ImportantDatePets_ImportantDates_ImportantDateId",
                        column: x => x.ImportantDateId,
                        principalTable: "ImportantDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportantDatePets_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_NoteId",
                table: "Examinations",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportantDatePets_PetId",
                table: "ImportantDatePets",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportantDates_NoteId",
                table: "ImportantDates",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_Notes_NoteId",
                table: "Examinations",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
