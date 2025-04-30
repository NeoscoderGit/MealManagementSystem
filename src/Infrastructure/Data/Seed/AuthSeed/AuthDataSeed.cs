using Domain.Entitys.AuthModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed.AuthSeed
{
    public static class AuthDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Roles
            var adminRole = new Role { Id = 1, Name = "Admin" };
            var userRole = new Role { Id = 2, Name = "User" };
            var hrRole = new Role { Id = 3, Name = "HR" };

            modelBuilder.Entity<Role>().HasData(adminRole, userRole, hrRole);

            // Menus
            var dashboardMenu = new Menu
            {
                Id = 1,
                Name = "Dashboard",
                Url = "/dashboard",
                IsParrent = true,
                IsLastChild = false,
                ParentId = null
            };

            var usersMenu = new Menu
            {
                Id = 2,
                Name = "Users",
                Url = "/users",
                IsParrent = false,
                IsLastChild = true,
                ParentId = 1 // child of Dashboard
            };

            var settingsMenu = new Menu
            {
                Id = 3,
                Name = "Settings",
                Url = "/settings",
                IsParrent = true,
                IsLastChild = true,
                ParentId = null
            };

            modelBuilder.Entity<Menu>().HasData(dashboardMenu, usersMenu, settingsMenu);

            // RoleMenus
            modelBuilder.Entity<RoleMenu>().HasData(
                new RoleMenu { Id = 1, RoleId = adminRole.Id, MenuId = dashboardMenu.Id },
                new RoleMenu { Id = 2, RoleId = adminRole.Id, MenuId = usersMenu.Id },
                new RoleMenu { Id = 3, RoleId = userRole.Id, MenuId = dashboardMenu.Id },
                new RoleMenu { Id = 4, RoleId = hrRole.Id, MenuId = settingsMenu.Id }
            );

            // Users
            var user1 = new User
            {
                Id = 1,
                EmployeeId = 1001,
                EmployeeName = "System Admin",
                EmployeeCode = "EMP001",
                DepartmentId = 1,
                DesignationId = 1,
                JobStationId = 1,
                UnitId = 1,
                OfficeEmail = "admin@company.com",
                ContactNo = "01711111111",
                Password = "Admin@123",
                CardNumber = "12346"
            };

            var user2 = new User
            {
                Id = 2,
                EmployeeId = 1002,
                EmployeeName = "John Doe",
                EmployeeCode = "EMP002",
                DepartmentId = 2,
                DesignationId = 2,
                JobStationId = 2,
                UnitId = 2,
                OfficeEmail = "john.doe@company.com",
                ContactNo = "01722222222",
                Password = "User@123",
                CardNumber = "12345"
            };

            var user3 = new User
            {
                Id = 3,
                EmployeeId = 1003,
                EmployeeName = "Jane Smith",
                EmployeeCode = "EMP003",
                DepartmentId = 3,
                DesignationId = 3,
                JobStationId = 3,
                UnitId = 3,
                OfficeEmail = "jane.smith@company.com",
                ContactNo = "01733333333",
                Password = "HR@123",
                CardNumber = "1234"
            };

            modelBuilder.Entity<User>().HasData(user1, user2, user3);

            // UserRoles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = user1.Id, RoleId = adminRole.Id },
                new UserRole { Id = 2, UserId = user2.Id, RoleId = userRole.Id },
                new UserRole { Id = 3, UserId = user3.Id, RoleId = hrRole.Id }
            );
        }
    }
}
