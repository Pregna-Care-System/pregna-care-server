using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Migrations
{
    /// <inheritdoc />
    public partial class ModifyFieldsInBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Status",
                table: "Blog");
        }
    }
}
