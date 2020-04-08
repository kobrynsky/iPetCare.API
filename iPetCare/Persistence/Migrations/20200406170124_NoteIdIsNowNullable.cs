using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class NoteIdIsNowNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                            name: "FK_ImportantDates_Notes_NoteId",
                            table: "ImportantDates");

            migrationBuilder.AlterColumn<Guid>(
                name: "NoteId",
                table: "ImportantDates",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_ImportantDates_Notes_NoteId",
               table: "ImportantDates");

            migrationBuilder.AlterColumn<Guid>(
                name: "NoteId",
                table: "ImportantDates",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
