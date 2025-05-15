using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeMealConfigurationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeMealDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Breakfast = table.Column<double>(type: "float", nullable: true),
                    BreakfastGuest = table.Column<double>(type: "float", nullable: true),
                    Lunch = table.Column<double>(type: "float", nullable: true),
                    LunchGuest = table.Column<double>(type: "float", nullable: true),
                    Dinner = table.Column<double>(type: "float", nullable: true),
                    DinnerGuest = table.Column<double>(type: "float", nullable: true),
                    IsSundayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsMondayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsTuesdayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsWednesdayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsThursdayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsFridaydayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsSaturdayMeal = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMealDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeMealDays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeMealDays_UserId",
                table: "EmployeeMealDays",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeMealDays");
        }
    }
}
