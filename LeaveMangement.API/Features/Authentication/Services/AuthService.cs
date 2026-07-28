using LeaveManagement.API.Common.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.API.Features.Authentication.Interfaces;
using LeaveManagement.API.Features.Authentication.Dtos;
using LeaveManagement.API.Features.Employees.Interfaces;
using LeaveManagement.API.Features.LeaveManagement.Interfaces;
using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Domain.Enums;
using LeaveManagement.API.Infrastructure.Persistence;
using LeaveManagement.API.Infrastructure.Configurations;

namespace LeaveManagement.API.Features.Authentication.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        private readonly ITokenService _tokenService;

        private readonly ApplicationDbContext _context;



        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmployeeRepository employeeRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            ITokenService tokenService,
            ApplicationDbContext context)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _employeeRepository = employeeRepository;

            _leaveBalanceRepository = leaveBalanceRepository;

            _tokenService = tokenService;

            _context = context;
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

                IsManager = false
            };



            await _employeeRepository
                .AddAsync(employee);


            await _employeeRepository
                .SaveChangesAsync();





            // =========================
            // Active Fiscal Year
            // =========================

            var activeFiscalYear =
                await _context.FiscalYears
                .FirstOrDefaultAsync(f =>
                    f.IsActive);



            if (activeFiscalYear == null)
            {
                throw new Exception(
                    "No active fiscal year found.");
            }





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

                        FiscalYearId =
                            activeFiscalYear.Id
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