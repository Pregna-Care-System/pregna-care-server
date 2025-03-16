using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedChartAndReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "SharedChartData",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Reactions", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Reactions_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_Reactions_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_Reactions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reactions_BlogId",
                table: "Reactions",
                column: "BlogId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Reactions");

            _ = migrationBuilder.DropColumn(
                name: "SharedChartData",
                table: "Blog");

            _ = migrationBuilder.DropColumn(
                name: "Type",
                table: "Blog");
        }
    }
}
