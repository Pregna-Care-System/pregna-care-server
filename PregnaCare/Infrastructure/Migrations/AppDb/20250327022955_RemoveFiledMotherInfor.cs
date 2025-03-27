using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class RemoveFiledMotherInfor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "MotherInfo");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "MotherInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
        name: "MotherName",
        table: "MotherInfo",
        type: "nvarchar(50)",
        nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "MotherInfo",
                type: "date",
                nullable: true);
        }
    }
}
