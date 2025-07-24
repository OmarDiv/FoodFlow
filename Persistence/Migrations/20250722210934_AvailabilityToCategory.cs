using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AvailabilityToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Categories");
        }
    }
}
