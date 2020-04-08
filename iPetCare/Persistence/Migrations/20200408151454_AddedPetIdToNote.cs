using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedPetIdToNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Pets_PetId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_PetId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "PetId",
                table: "Notes",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PetId1",
                table: "Notes",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "NoteId",
                table: "ImportantDates",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PetId1",
                table: "Notes",
                column: "PetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Pets_PetId1",
                table: "Notes",
                column: "PetId1",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Pets_PetId1",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_PetId1",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PetId1",
                table: "Notes");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "NoteId",
                table: "ImportantDates",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PetId",
                table: "Notes",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantDates_Notes_NoteId",
                table: "ImportantDates",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Pets_PetId",
                table: "Notes",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
