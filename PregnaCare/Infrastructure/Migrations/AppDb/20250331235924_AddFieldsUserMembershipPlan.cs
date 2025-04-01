using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddFieldsUserMembershipPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentErrorCode",
                table: "UserMembershipPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "UserMembershipPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "UserMembershipPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StatusNotes",
                table: "UserMembershipPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentErrorCode",
                table: "UserMembershipPlan");

            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "UserMembershipPlan");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "UserMembershipPlan");

            migrationBuilder.DropColumn(
                name: "StatusNotes",
                table: "UserMembershipPlan");
        }
    }
}
