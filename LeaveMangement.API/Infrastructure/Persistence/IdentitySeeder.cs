using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Infrastructure.Persistence
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            SeedAdminSettings seedAdminSettings)
        {
            Console.WriteLine("===== Seeding Identity =====");

            await SeedRolesAsync(roleManager);

            await SeedPermissionsAsync(context);

            await SeedAdminAsync(
                userManager,
                seedAdminSettings);

            await SeedRolePermissionsAsync(
                roleManager,
                context);

            Console.WriteLine("===== Identity Seeding Completed =====");
        }


        // =====================================================
        // ROLES
        // =====================================================

        private static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "Manager",
                "Employee"
            };


            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }



        // =====================================================
        // PERMISSIONS
        // =====================================================

        private static async Task SeedPermissionsAsync(
            ApplicationDbContext context)
        {
            var permissions = new List<Permission>
            {
                new Permission
                {
                    Name = "Leave.Apply",
                    Description = "Apply leave request"
                },

                new Permission
                {
                    Name = "Leave.ViewOwn",
                    Description = "View own leave requests"
                },

                new Permission
                {
                    Name = "Leave.ViewBalance",
                    Description = "View own leave balances"
                },

                new Permission
                {
                    Name = "Leave.Cancel",
                    Description = "Cancel own leave request"
                },

                new Permission
                {
                    Name = "Leave.ViewPending",
                    Description = "View pending leave requests"
                },

                new Permission
                {
                    Name = "Leave.Approve",
                    Description = "Approve leave requests"
                },

                new Permission
                {
                    Name = "Leave.Reject",
                    Description = "Reject leave requests"
                },

                new Permission
                {
                    Name = "Employee.View",
                    Description = "View employees"
                },

                new Permission
                {
                    Name = "Employee.Create",
                    Description = "Create employees"
                },

                new Permission
                {
                    Name = "Employee.AssignManager",
                    Description = "Assign manager to employee"
                },

                new Permission
                {
                    Name = "Manager.Create",
                    Description = "Create managers"
                }
            };


            foreach (var permission in permissions)
            {
                bool exists =
                    await context.Permissions
                    .AnyAsync(x =>
                        x.Name == permission.Name);


                if (!exists)
                {
                    await context.Permissions.AddAsync(permission);
                }
            }


            await context.SaveChangesAsync();
        }
        // =====================================================
        // DEFAULT ADMIN
        // =====================================================

        private static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager,
            SeedAdminSettings seedAdminSettings)
        {
            var admin =
                await userManager
                .FindByEmailAsync(
                    seedAdminSettings.Email);


            if (admin != null)
                return;



            admin = new ApplicationUser
            {
                UserName =
                    seedAdminSettings.Email,

                Email =
                    seedAdminSettings.Email,

                EmailConfirmed = true,

                IsActive = true,

                CreatedAt =
                    DateTime.UtcNow
            };



            var result =
                await userManager.CreateAsync(
                    admin,
                    seedAdminSettings.Password);



            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                    result.Errors
                    .Select(e =>
                        e.Description)));
            }



            await userManager.AddToRoleAsync(
                admin,
                "Admin");
        }





        // =====================================================
        // ROLE PERMISSIONS
        // =====================================================

        private static async Task SeedRolePermissionsAsync(
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {

            var adminRole =
                await roleManager.FindByNameAsync(
                    "Admin");


            var managerRole =
                await roleManager.FindByNameAsync(
                    "Manager");


            var employeeRole =
                await roleManager.FindByNameAsync(
                    "Employee");



            if (adminRole == null ||
                managerRole == null ||
                employeeRole == null)
            {
                throw new Exception(
                    "Required roles were not found.");
            }



            var permissions =
                await context.Permissions
                .ToListAsync();




            async Task AddPermissionAsync(
                IdentityRole role,
                string permissionName)
            {

                var permission =
                    permissions.First(p =>
                        p.Name == permissionName);



                bool exists =
                    await context.RolePermissions
                    .AnyAsync(rp =>
                        rp.RoleId == role.Id
                        &&
                        rp.PermissionId ==
                        permission.Id);



                if (!exists)
                {
                    await context.RolePermissions
                    .AddAsync(
                        new RolePermission
                        {
                            RoleId =
                                role.Id,

                            PermissionId =
                                permission.Id
                        });
                }
            }





            // ==========================
            // ADMIN PERMISSIONS
            // ==========================

            foreach (var permission in new[]
            {
                "Employee.View",
                "Employee.Create",
                "Employee.AssignManager",
                "Manager.Create"
            })
            {
                await AddPermissionAsync(
                    adminRole,
                    permission);
            }





            // ==========================
            // MANAGER PERMISSIONS
            // ==========================

            foreach (var permission in new[]
            {
                "Leave.ViewPending",
                "Leave.Approve",
                "Leave.Reject"
            })
            {
                await AddPermissionAsync(
                    managerRole,
                    permission);
            }





            // ==========================
            // EMPLOYEE PERMISSIONS
            // ==========================

            foreach (var permission in new[]
            {
                "Leave.Apply",
                "Leave.ViewOwn",
                "Leave.ViewBalance",
                "Leave.Cancel"
            })
            {
                await AddPermissionAsync(
                    employeeRole,
                    permission);
            }



            await context.SaveChangesAsync();

        }

    }
}