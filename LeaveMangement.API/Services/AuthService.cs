using LeaveManagement.API.Enums;
using LeaveManagement.API.Exceptions;
using LeaveManagement.API.Models;
using LeaveManagement.API.Configurations;
using Microsoft.AspNetCore.Identity;
using LeaveMangement.API.DTOs;
using LeaveMangement.API.Interfaces;

namespace LeaveMangement.API.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        private readonly ITokenService _tokenService;



        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmployeeRepository employeeRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            ITokenService tokenService)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _employeeRepository = employeeRepository;

            _leaveBalanceRepository = leaveBalanceRepository;

            _tokenService = tokenService;
        }




        public async Task RegisterAsync(
            RegisterRequestDto request)
        {

            var existingUser =
                await _userManager
                .FindByEmailAsync(request.Email);



            if (existingUser != null)
            {
                throw new BadRequestException(
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
                throw new BadRequestException(
                    string.Join(", ",
                    result.Errors.Select(
                        e => e.Description)));
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

                ManagerId = null,

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







        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request)
        {


            var user =
                await _userManager
                .FindByEmailAsync(request.Email);



            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }




            var valid =
                await _userManager
                .CheckPasswordAsync(
                    user,
                    request.Password);



            if (!valid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }






            var roles =
                await _userManager
                .GetRolesAsync(user);



            var role =
                roles.First();




            int employeeId = 0;




            // Admin is not an employee
            // Manager and Employee have Employee records

            if (role != "Admin")
            {

                var employee =
                    await _employeeRepository
                    .GetByUserIdAsync(user.Id);



                if (employee == null)
                {
                    throw new Exception(
                        "Employee record not found.");
                }


                employeeId = employee.Id;

            }







            var token =
                _tokenService.GenerateToken(
                    user.Id,
                    employeeId,
                    user.Email!,
                    role);






            return new LoginResponseDto
            {
                Token = token,

                Email = user.Email!,

                Role = role
            };

        }

    }
}