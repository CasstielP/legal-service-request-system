using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LegalServiceRequest.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@county.local", "Admin User", "Admin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "staff@county.local", "Staff User", "Staff" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "developer@county.local", "Developer User", "Developer" }
                });

            migrationBuilder.InsertData(
                table: "ServiceRequests",
                columns: new[] { "Id", "AssignedToUserId", "CreatedAt", "CreatedByUserId", "Department", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, "County Counsel", "County Counsel staff requested an update to internal policy reference content on the department intranet.", new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", "Open", "Update department intranet content", null },
                    { 2, 3, new DateTime(2026, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Technology & Business Solutions", "A staff user reported that they are unable to access the internal request portal after password reset.", new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "High", "In Progress", "Investigate login issue", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
