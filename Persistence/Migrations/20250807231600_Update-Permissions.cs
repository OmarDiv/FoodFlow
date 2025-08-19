using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 23, "permissions", "roles:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 24, "permissions", "roles:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 25, "permissions", "roles:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 26, "permissions", "roles:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 27, "permissions", "roles:assign-permissions", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 28, "permissions", "promotions:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 29, "permissions", "promotions:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 30, "permissions", "promotions:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 31, "permissions", "promotions:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 32, "permissions", "promotions:toggle-status", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 33, "permissions", "promotions:assign-restaurant", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 34, "permissions", "delivery-zones:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 35, "permissions", "delivery-zones:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 36, "permissions", "delivery-zones:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 37, "permissions", "delivery-zones:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 38, "permissions", "Dlivery:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 39, "permissions", "Dlivery:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 40, "permissions", "Dlivery:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 41, "permissions", "Dlivery:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 42, "permissions", "users:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 43, "permissions", "users:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 44, "permissions", "users:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 45, "permissions", "users:assign-roles", "01986241-4662-71f4-95b6-31ac4820553f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45);
        }
    }
}
