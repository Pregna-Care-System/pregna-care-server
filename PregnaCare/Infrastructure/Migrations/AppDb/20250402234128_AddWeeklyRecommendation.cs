using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddWeeklyRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PregnancyRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NutritionalAdvice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthConcerns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BabyDevelopment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyRecommendations_PregnancyRecord_PregnancyRecordId",
                        column: x => x.PregnancyRecordId,
                        principalTable: "PregnancyRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyRecommendations_PregnancyRecordId",
                table: "WeeklyRecommendations",
                column: "PregnancyRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyRecommendations");
        }
    }
}
