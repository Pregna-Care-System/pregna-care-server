﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Feature",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Route",
                table: "Feature");
        }
    }
}
