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
            _ = migrationBuilder.DropColumn(
                name: "MotherName",
                table: "MotherInfo");

            _ = migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "MotherInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
        name: "MotherName",
        table: "MotherInfo",
        type: "nvarchar(50)",
        nullable: true);

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "MotherInfo",
                type: "date",
                nullable: true);
        }
    }
}
