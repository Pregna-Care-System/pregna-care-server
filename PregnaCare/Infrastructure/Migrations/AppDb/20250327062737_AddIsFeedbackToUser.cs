using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddIsFeedbackToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsFeedback",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsFeedback",
                table: "User");
        }
    }
}
