using LeaveManagement.API.Models;
using LeaveManagement.API.Enums;
using LeaveManagement.API.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace LeaveManagement.API.Data
{
    public static class IdentitySeeder
    {

        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {

            Console.WriteLine("===== Seeding Role Permissions =====");

            // =====================
            // Roles
            // =====================

            string[] roles =
            {
                "Admin",
                "Employee",
                "Manager"
            };


            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {

                    await roleManager.CreateAsync(
                        new IdentityRole(role));

                }

            }

            // =====================
            // Permissions
            // =====================

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
                    .AnyAsync(p =>
                        p.Name == permission.Name);


                if (!exists)
                {
                    await context.Permissions.AddAsync(permission);
                }
            }


            await context.SaveChangesAsync();



            // =====================
            // Admin User
            // =====================

            var adminEmail =
                "admin@test.com";


            var admin =
                await userManager
                .FindByEmailAsync(adminEmail);



            if (admin == null)
            {

                admin = new ApplicationUser
                {
                    UserName = adminEmail,

                    Email = adminEmail,

                    EmailConfirmed = true,

                    IsActive = true,

                    CreatedAt = DateTime.UtcNow
                };


                await userManager.CreateAsync(
                    admin,
                    "Admin@123");


                await userManager.AddToRoleAsync(
                    admin,
                    "Admin");

            }

            // =====================
            // Role Permissions
            // =====================


            var adminRole =
                await roleManager.FindByNameAsync("Admin");


            var managerRole =
                await roleManager.FindByNameAsync("Manager");


            var employeeRole =
                await roleManager.FindByNameAsync("Employee");



            var allPermissions =
                await context.Permissions.ToListAsync();



            async Task AddPermissionToRole(
                IdentityRole role,
                string permissionName)
            {

                var permission =
                    allPermissions
                    .First(p =>
                        p.Name == permissionName);



                bool exists =
                    await context.RolePermissions
                    .AnyAsync(rp =>
                        rp.RoleId == role.Id
                        &&
                        rp.PermissionId == permission.Id);



                if (!exists)
                {
                    await context.RolePermissions.AddAsync(
                        new RolePermission
                        {
                            RoleId = role.Id,

                            PermissionId = permission.Id
                        });
                }

            }




            // Admin permissions

            foreach (var permission in new[]
 {
    "Employee.View",
    "Employee.Create",
    "Employee.AssignManager",
    "Manager.Create"
})
            {
                await AddPermissionToRole(
                    adminRole!,
                    permission);
            }





            // Manager permissions

            foreach (var permission in new[]
            {
    "Leave.ViewPending",
    "Leave.Approve",
    "Leave.Reject"
})
            {
                await AddPermissionToRole(
                    managerRole!,
                    permission);
            }





            // Employee permissions

            foreach (var permission in new[]
{
    "Leave.Apply",
    "Leave.ViewOwn",
    "Leave.ViewBalance",
    "Leave.Cancel"
})
            {
                await AddPermissionToRole(
                    employeeRole!,
                    permission);
            }



            await context.SaveChangesAsync();




            // =====================
            // Manager User
            // =====================


            var managerEmail =
                "manager@test.com";


            var manager =
                await userManager
                .FindByEmailAsync(managerEmail);



            if (manager == null)
            {

                manager = new ApplicationUser
                {
                    UserName = managerEmail,

                    Email = managerEmail,

                    EmailConfirmed = true,

                    IsActive = true,

                    CreatedAt = DateTime.UtcNow
                };


                await userManager.CreateAsync(
                    manager,
                    "Manager@123");


                await userManager.AddToRoleAsync(
                    manager,
                    "Manager");

            }





            // =====================
            // Employee User
            // =====================


            var employeeEmail =
                "ahmed@test.com";


            var employeeUser =
                await userManager
                .FindByEmailAsync(employeeEmail);



            if (employeeUser == null)
            {

                employeeUser = new ApplicationUser
                {
                    UserName = employeeEmail,

                    Email = employeeEmail,

                    EmailConfirmed = true,

                    IsActive = true,

                    CreatedAt = DateTime.UtcNow
                };


                await userManager.CreateAsync(
                    employeeUser,
                    "Ahmed@123");


                await userManager.AddToRoleAsync(
                    employeeUser,
                    "Employee");

            }






            // =====================
            // Manager Employee Record
            // =====================


            var managerEmployee =
                await context.Employees
                .FirstOrDefaultAsync(e =>
                    e.UserId == manager.Id);



            if (managerEmployee == null)
            {

                managerEmployee = new Employee
                {
                    UserId = manager.Id,

                    FullName = "Ali Manager",

                    Department = "IT",

                    Designation = "Project Manager",

                    IsManager = true
                };


                await context.Employees.AddAsync(
                    managerEmployee);


                await context.SaveChangesAsync();

            }







            // =====================
            // Ahmed Employee Record
            // =====================


            var ahmedEmployee =
                await context.Employees
                .FirstOrDefaultAsync(e =>
                    e.UserId == employeeUser.Id);



            if (ahmedEmployee == null)
            {

                ahmedEmployee = new Employee
                {
                    UserId = employeeUser.Id,

                    FullName = "Ahmed Raza",

                    Department = "Software Development",

                    Designation = "Junior .NET Developer",

                    IsManager = false
                };


                await context.Employees.AddAsync(
                    ahmedEmployee);


                await context.SaveChangesAsync();

            }


            var assignmentExists =
    await context.EmployeeManagerAssignments
    .AnyAsync(a =>
        a.EmployeeId == ahmedEmployee.Id &&
        a.ManagerId == managerEmployee.Id);



            if (!assignmentExists)
            {
                context.EmployeeManagerAssignments.Add(
                    new EmployeeManagerAssignment
                    {
                        EmployeeId = ahmedEmployee.Id,

                        ManagerId = managerEmployee.Id,

                        IsActive = true,

                        AssignedOn = DateTime.UtcNow
                    });
            }




            // =====================
            // Leave Balances
            // =====================


            var leaveTypes =
                Enum.GetValues<LeaveType>();




            foreach (var employee in new[]
            {
                managerEmployee,
                ahmedEmployee
            })
            {


                foreach (var type in leaveTypes)
                {


                    bool exists =
                        await context.LeaveBalances
                        .AnyAsync(x =>
                            x.EmployeeId == employee.Id
                            &&
                            x.LeaveType == type
                            &&
                            x.Year == DateTime.UtcNow.Year);



                    if (!exists)
                    {

                        double balance =
                            LeavePolicy
                            .GetDefaultDays(type);



                        context.LeaveBalances.Add(
                            new LeaveBalance
                            {

                                EmployeeId =
                                    employee.Id,


                                LeaveType =
                                    type,


                                TotalBalance =
                                    balance,


                                ConsumedBalance =
                                    0,


                                RemainingBalance =
                                    balance,


                                Year =
                                    DateTime.UtcNow.Year

                            });

                    }

                }

            }



            await context.SaveChangesAsync();

        }

    }
}