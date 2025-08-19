using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoldesAndClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01986241-4662-71f4-95b6-31ab855ae411", "0198680f-89d0-7335-8307-a378defa9109", false, false, "RestaurantOwner", "RESTAURANTOWNER" },
                    { "01986241-4662-71f4-95b6-31ac4820553f", "0198680f-89d0-7335-8307-a348cee60de4", false, false, "Admin", "ADMIN" },
                    { "01986241-4662-71f4-95b6-31b2d584c3ad", "0198680f-89d0-7335-8307-a347826a8c88", true, false, "Customer", "CUSTOMER" },
                    { "01986241-4662-71f4-95b6-31bb2a547123", "0198680f-89d0-7335-8307-a333865a77f1", false, false, "Delivery", "DELIVERY" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "restaurants:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 2, "permissions", "restaurants:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 3, "permissions", "restaurants:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 4, "permissions", "restaurants:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 5, "permissions", "restaurants:toggle-open", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 6, "permissions", "restaurants:toggle-active", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 7, "permissions", "categories:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 8, "permissions", "categories:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 9, "permissions", "categories:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 10, "permissions", "categories:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 11, "permissions", "categories:toggle-status", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 12, "permissions", "menu-items:read", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 13, "permissions", "menu-items:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 14, "permissions", "menu-items:update", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 15, "permissions", "menu-items:delete", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 16, "permissions", "menu-items:toggle-availability", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 17, "permissions", "orders:create", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 18, "permissions", "orders:cancel", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 19, "permissions", "orders:view", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 20, "permissions", "orders:view-customer", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 21, "permissions", "orders:view-restaurant", "01986241-4662-71f4-95b6-31ac4820553f" },
                    { 22, "permissions", "orders:update-status", "01986241-4662-71f4-95b6-31ac4820553f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01986241-4662-71f4-95b6-31ab855ae411");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01986241-4662-71f4-95b6-31b2d584c3ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01986241-4662-71f4-95b6-31bb2a547123");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01986241-4662-71f4-95b6-31ac4820553f");
        }
    }
}
