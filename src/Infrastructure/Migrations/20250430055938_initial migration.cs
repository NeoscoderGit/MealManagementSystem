using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakFastTimeStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    BreakFastTimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    LunchTimeStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    LunchTimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    DinnerTimeStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    DinnerTimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsParrent = table.Column<bool>(type: "bit", nullable: true),
                    IsLastChild = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: true),
                    JobStationId = table.Column<int>(type: "int", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    OfficeEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Breakfast = table.Column<double>(type: "float", nullable: true),
                    BreakfastGuest = table.Column<double>(type: "float", nullable: true),
                    BreakTotalTaken = table.Column<double>(type: "float", nullable: true),
                    IsBreakfastComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsBreakfastTaken = table.Column<bool>(type: "bit", nullable: true),
                    Lunch = table.Column<double>(type: "float", nullable: true),
                    LunchGuest = table.Column<double>(type: "float", nullable: true),
                    LunchTotalTaken = table.Column<double>(type: "float", nullable: true),
                    IsLunchTaken = table.Column<bool>(type: "bit", nullable: true),
                    IsLunchComplete = table.Column<bool>(type: "bit", nullable: true),
                    Dinner = table.Column<double>(type: "float", nullable: true),
                    DinnerGuest = table.Column<double>(type: "float", nullable: true),
                    DinnerTotalTaken = table.Column<double>(type: "float", nullable: true),
                    IsDinnerComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsDinnerTaken = table.Column<bool>(type: "bit", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "DATEFROMPARTS([Year], [Month], [Day])", stored: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMeals", x => x.Id);
                    table.CheckConstraint("CK_DailyMeal_PositiveMeals", "(Breakfast IS NULL OR Breakfast >= 0) AND (Lunch IS NULL OR Lunch >= 0) AND (Dinner IS NULL OR Dinner >= 0) AND (BreakfastGuest IS NULL OR BreakfastGuest >= 0) AND (LunchGuest IS NULL OR LunchGuest >= 0) AND (DinnerGuest IS NULL OR DinnerGuest >= 0)");
                    table.CheckConstraint("CK_DailyMeal_ValidDate", "Year BETWEEN 2000 AND 2100 AND Month BETWEEN 1 AND 12 AND Day BETWEEN 1 AND 31 AND (Day <= CASE     WHEN Month IN (1,3,5,7,8,10,12) THEN 31     WHEN Month IN (4,6,9,11) THEN 30     WHEN Year % 4 = 0 AND (Year % 100 != 0 OR Year % 400 = 0) THEN 29     ELSE 28 END)");
                    table.ForeignKey(
                        name: "FK_DailyMeals_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MealTimes",
                columns: new[] { "Id", "BreakFastTimeEnd", "BreakFastTimeStart", "CreatedBy", "CreatedDate", "DinnerTimeEnd", "DinnerTimeStart", "LunchTimeEnd", "LunchTimeStart", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, new TimeOnly(11, 0, 0), new TimeOnly(8, 0, 0), null, null, new TimeOnly(23, 0, 0), new TimeOnly(19, 0, 0), new TimeOnly(16, 0, 0), new TimeOnly(12, 0, 0), null, null });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedBy", "IsLastChild", "IsParrent", "Name", "ParentId", "UpdatedBy", "UpdatedDate", "Url" },
                values: new object[,]
                {
                    { 1, null, false, true, "Dashboard", null, null, null, "/dashboard" },
                    { 3, null, true, true, "Settings", null, null, null, "/settings" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, "Admin", null, null },
                    { 2, null, "User", null, null },
                    { 3, null, "HR", null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CardNumber", "ContactNo", "CreatedBy", "DepartmentId", "DesignationId", "EmployeeCode", "EmployeeId", "EmployeeName", "JobStationId", "OfficeEmail", "Password", "UnitId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "12346", "01711111111", null, 1, 1, "EMP001", 1001, "System Admin", 1, "admin@company.com", "Admin@123", 1, null, null },
                    { 2, "12345", "01722222222", null, 2, 2, "EMP002", 1002, "John Doe", 2, "john.doe@company.com", "User@123", 2, null, null },
                    { 3, "1234", "01733333333", null, 3, 3, "EMP003", 1003, "Jane Smith", 3, "jane.smith@company.com", "HR@123", 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedBy", "IsLastChild", "IsParrent", "Name", "ParentId", "UpdatedBy", "UpdatedDate", "Url" },
                values: new object[] { 2, null, true, false, "Users", 1, null, null, "/users" });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "Id", "CreatedBy", "MenuId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, 1, 1, null, null },
                    { 3, null, 1, 2, null, null },
                    { 4, null, 3, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedBy", "RoleId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, null, null, 1 },
                    { 2, null, 2, null, null, 2 },
                    { 3, null, 3, null, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "Id", "CreatedBy", "MenuId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 2, null, 2, 1, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_Date",
                table: "DailyMeals",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_EmployeeId",
                table: "DailyMeals",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_EmployeeId_Year_Month_Day",
                table: "DailyMeals",
                columns: new[] { "EmployeeId", "Year", "Month", "Day" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_Year_Month",
                table: "DailyMeals",
                columns: new[] { "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_Year_Month_Day",
                table: "DailyMeals",
                columns: new[] { "Year", "Month", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_MenuId",
                table: "RoleMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMeals");

            migrationBuilder.DropTable(
                name: "MealTimes");

            migrationBuilder.DropTable(
                name: "RoleMenus");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
