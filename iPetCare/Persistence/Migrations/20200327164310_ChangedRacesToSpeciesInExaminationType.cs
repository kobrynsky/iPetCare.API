using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ChangedRacesToSpeciesInExaminationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTypes_Races_RaceId",
                table: "ExaminationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExaminationTypes_RaceId",
                table: "ExaminationTypes");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "ExaminationTypes");

            migrationBuilder.AddColumn<int>(
                name: "SpeciesId",
                table: "ExaminationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTypes_SpeciesId",
                table: "ExaminationTypes",
                column: "SpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTypes_Species_SpeciesId",
                table: "ExaminationTypes",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTypes_Species_SpeciesId",
                table: "ExaminationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExaminationTypes_SpeciesId",
                table: "ExaminationTypes");

            migrationBuilder.DropColumn(
                name: "SpeciesId",
                table: "ExaminationTypes");

            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "ExaminationTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTypes_RaceId",
                table: "ExaminationTypes",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTypes_Races_RaceId",
                table: "ExaminationTypes",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
