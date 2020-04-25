using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedPetToNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notes_Pets_PetId1",
            //    table: "Notes");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notes_PetId1",
            //    table: "Notes");

            //migrationBuilder.DropColumn(
            //    name: "PetId1",
            //    table: "Notes");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "PetId",
            //    table: "Notes",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notes_PetId",
            //    table: "Notes",
            //    column: "PetId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notes_Pets_PetId",
            //    table: "Notes",
            //    column: "PetId",
            //    principalTable: "Pets",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notes_Pets_PetId",
            //    table: "Notes");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notes_PetId",
            //    table: "Notes");

            //migrationBuilder.AlterColumn<string>(
            //    name: "PetId",
            //    table: "Notes",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(Guid));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "PetId1",
            //    table: "Notes",
            //    type: "uniqueidentifier",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notes_PetId1",
            //    table: "Notes",
            //    column: "PetId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notes_Pets_PetId1",
            //    table: "Notes",
            //    column: "PetId1",
            //    principalTable: "Pets",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
