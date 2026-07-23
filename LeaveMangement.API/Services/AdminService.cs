using LeaveManagement.API.Configurations;
using LeaveManagement.API.Enums;
using LeaveManagement.API.Models;
using LeaveMangement.API.DTOs;
using LeaveMangement.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.API.Services
{
    public class AdminService : IAdminService
    {

        private readonly IEmployeeRepository _employeeRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;



        public AdminService(
            IEmployeeRepository employeeRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILeaveBalanceRepository leaveBalanceRepository)
        {
            _employeeRepository = employeeRepository;

            _userManager = userManager;

            _roleManager = roleManager;

            _leaveBalanceRepository = leaveBalanceRepository;
        }






        public async Task<List<AdminEmployeeResponseDto>>
            GetAllEmployeesAsync()
        {

            var employees =
                await _employeeRepository
                .GetAllEmployeesAsync();



            return employees
                .Select(e =>
                    new AdminEmployeeResponseDto
                    {
                        Id = e.Id,

                        FullName = e.FullName,

                        Department = e.Department,

                        Designation = e.Designation,

                        ManagerName =
                            e.Manager != null
                            ? e.Manager.FullName
                            : null,

                        IsManager = e.IsManager

                    })
                .ToList();

        }







        public async Task CreateEmployeeAsync(
            CreateEmployeeRequestDto request)
        {


            var existingUser =
                await _userManager
                .FindByEmailAsync(request.Email);



            if (existingUser != null)
            {
                throw new Exception(
                    "Email already exists.");
            }





            var user = new ApplicationUser
            {
                UserName = request.Email,

                Email = request.Email,

                EmailConfirmed = true,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };





            var result =
                await _userManager
                .CreateAsync(
                    user,
                    request.Password);




            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                    result.Errors
                    .Select(x => x.Description)));
            }





            if (!await _roleManager
                .RoleExistsAsync("Employee"))
            {
                await _roleManager
                    .CreateAsync(
                    new IdentityRole("Employee"));
            }





            await _userManager
                .AddToRoleAsync(
                    user,
                    "Employee");







            var employee = new Employee
            {
                UserId = user.Id,

                FullName = request.FullName,

                Department = request.Department,

                Designation = request.Designation,

                ManagerId = request.ManagerId,

                IsManager = false
            };





            await _employeeRepository
                .AddAsync(employee);


            await _employeeRepository
                .SaveChangesAsync();








            var balances =
                Enum.GetValues<LeaveType>()
                .Select(type =>
                    new LeaveBalance
                    {
                        EmployeeId = employee.Id,

                        LeaveType = type,

                        TotalBalance =
                            LeavePolicy
                            .GetDefaultDays(type),

                        ConsumedBalance = 0,

                        RemainingBalance =
                            LeavePolicy
                            .GetDefaultDays(type),

                        Year =
                            DateTime.UtcNow.Year
                    })
                .ToList();





            await _leaveBalanceRepository
                .AddRangeAsync(balances);


            await _leaveBalanceRepository
                .SaveChangesAsync();

        }



        public async Task CreateManagerAsync(
    CreateManagerRequestDto request)
        {

            var existingUser =
                await _userManager
                .FindByEmailAsync(request.Email);



            if (existingUser != null)
            {
                throw new Exception(
                    "Email already exists.");
            }





            var user = new ApplicationUser
            {
                UserName = request.Email,

                Email = request.Email,

                EmailConfirmed = true,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };





            var result =
                await _userManager
                .CreateAsync(
                    user,
                    request.Password);





            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                    result.Errors
                    .Select(x => x.Description)));
            }





            if (!await _roleManager
                .RoleExistsAsync("Manager"))
            {
                await _roleManager
                    .CreateAsync(
                    new IdentityRole("Manager"));
            }





            await _userManager
                .AddToRoleAsync(
                    user,
                    "Manager");







            var employee = new Employee
            {
                UserId = user.Id,

                FullName = request.FullName,

                Department = request.Department,

                Designation = request.Designation,

                ManagerId = null,

                IsManager = true
            };





            await _employeeRepository
                .AddAsync(employee);


            await _employeeRepository
                .SaveChangesAsync();








            var balances =
                Enum.GetValues<LeaveType>()
                .Select(type =>
                    new LeaveBalance
                    {
                        EmployeeId = employee.Id,

                        LeaveType = type,

                        TotalBalance =
                            LeavePolicy
                            .GetDefaultDays(type),

                        ConsumedBalance = 0,

                        RemainingBalance =
                            LeavePolicy
                            .GetDefaultDays(type),

                        Year =
                            DateTime.UtcNow.Year
                    })
                .ToList();





            await _leaveBalanceRepository
                .AddRangeAsync(balances);


            await _leaveBalanceRepository
                .SaveChangesAsync();

        }



        public async Task AssignManagerAsync(
    int employeeId,
    AssignManagerRequestDto request)
        {

            var employee =
                await _employeeRepository
                .GetByIdAsync(employeeId);



            if (employee == null)
            {
                throw new Exception(
                    "Employee not found.");
            }



            var manager =
                await _employeeRepository
                .GetByIdAsync(request.ManagerId);



            if (manager == null)
            {
                throw new Exception(
                    "Manager not found.");
            }



            if (!manager.IsManager)
            {
                throw new Exception(
                    "Selected employee is not a manager.");
            }



            if (employeeId == request.ManagerId)
            {
                throw new Exception(
                    "Employee cannot be their own manager.");
            }



            employee.ManagerId = request.ManagerId;


            await _employeeRepository
                .SaveChangesAsync();

        }

    }
}