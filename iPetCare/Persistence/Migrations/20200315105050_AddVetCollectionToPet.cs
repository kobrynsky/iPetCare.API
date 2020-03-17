using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddVetCollectionToPet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Vets_VetId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_VetId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "VetId",
                table: "Pets");

            migrationBuilder.CreateTable(
                name: "VetPets",
                columns: table => new
                {
                    VetId = table.Column<Guid>(nullable: false),
                    PetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetPets", x => new { x.VetId, x.PetId });
                    table.ForeignKey(
                        name: "FK_VetPets_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VetPets_Vets_VetId",
                        column: x => x.VetId,
                        principalTable: "Vets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VetPets_PetId",
                table: "VetPets",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VetPets");

            migrationBuilder.AddColumn<Guid>(
                name: "VetId",
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_VetId",
                table: "Pets",
                column: "VetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Vets_VetId",
                table: "Pets",
                column: "VetId",
                principalTable: "Vets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
