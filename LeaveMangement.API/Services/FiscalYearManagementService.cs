using LeaveManagement.API.Configurations;
using LeaveManagement.API.Enums;
using LeaveManagement.API.Exceptions;
using LeaveManagement.API.Models;
using LeaveMangement.API.DTOs;
using LeaveMangement.API.Interfaces;

namespace LeaveMangement.API.Services
{
    public class FiscalYearManagementService
        : IFiscalYearManagementService
    {
        private readonly IFiscalYearRepository _repository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;


        public FiscalYearManagementService(
            IFiscalYearRepository repository,
            IEmployeeRepository employeeRepository,
            ILeaveBalanceRepository leaveBalanceRepository)
        {
            _repository = repository;

            _employeeRepository = employeeRepository;

            _leaveBalanceRepository = leaveBalanceRepository;
        }



        // ==========================================
        // Get All Fiscal Years
        // ==========================================

        public async Task<List<FiscalYearResponseDto>>
            GetAllAsync()
        {
            var fiscalYears =
                await _repository.GetAllAsync();


            return fiscalYears

                .Select(x => new FiscalYearResponseDto
                {
                    Id = x.Id,

                    Name = x.Name,

                    StartDate = x.StartDate,

                    EndDate = x.EndDate,

                    IsActive = x.IsActive,

                    CreatedAt = x.CreatedAt

                })

                .ToList();
        }





        // ==========================================
        // Create Fiscal Year
        // ==========================================

        public async Task<FiscalYearResponseDto>
            CreateAsync(
                CreateFiscalYearDto request)
        {
            // Validate Dates

            if (request.EndDate <= request.StartDate)
            {
                throw new BadRequestException(
                    "End date must be after start date.");
            }



            // Duplicate Name

            bool nameExists =
                await _repository.NameExistsAsync(
                    request.Name);


            if (nameExists)
            {
                throw new BadRequestException(
                    "Fiscal year name already exists.");
            }




            // Date Overlap

            bool overlap =
                await _repository.HasOverlappingDatesAsync(
                    request.StartDate,
                    request.EndDate);


            if (overlap)
            {
                throw new BadRequestException(
                    "Fiscal year dates overlap with an existing fiscal year.");
            }




            var fiscalYear =
                new FiscalYear
                {
                    Name = request.Name,

                    StartDate = request.StartDate,

                    EndDate = request.EndDate,

                    IsActive = false,

                    CreatedAt = DateTime.UtcNow
                };



            await _repository.AddAsync(
                fiscalYear);



            await _repository.SaveChangesAsync();



            return new FiscalYearResponseDto
            {
                Id = fiscalYear.Id,

                Name = fiscalYear.Name,

                StartDate = fiscalYear.StartDate,

                EndDate = fiscalYear.EndDate,

                IsActive = fiscalYear.IsActive,

                CreatedAt = fiscalYear.CreatedAt
            };
        }





        // ==========================================
        // Activate Fiscal Year
        // ==========================================

        public async Task ActivateAsync(
            int fiscalYearId)
        {
            var fiscalYear =
                await _repository.GetByIdAsync(
                    fiscalYearId);



            if (fiscalYear == null)
            {
                throw new NotFoundException(
                    "Fiscal year not found.");
            }




            if (fiscalYear.IsActive)
            {
                throw new BadRequestException(
                    "This fiscal year is already active.");
            }




            var activeFiscalYear =
                await _repository.GetActiveAsync();



            if (activeFiscalYear != null)
            {
                activeFiscalYear.IsActive = false;
            }



            fiscalYear.IsActive = true;



            await _repository.SaveChangesAsync();




            // Create Leave Balances

            var employees =
                await _employeeRepository
                .GetAllEmployeesAsync();



            var leaveBalances =
                new List<LeaveBalance>();




            foreach (var employee in employees)
            {
                bool alreadyExists =
                    await _leaveBalanceRepository
                    .ExistsAsync(
                        employee.Id,
                        fiscalYear.Id);



                if (alreadyExists)
                {
                    continue;
                }




                foreach (var leaveType in Enum.GetValues<LeaveType>())
                {
                    var defaultBalance =
                        LeavePolicy.GetDefaultDays(
                            leaveType);



                    leaveBalances.Add(
                        new LeaveBalance
                        {
                            EmployeeId = employee.Id,

                            LeaveType = leaveType,

                            TotalBalance = defaultBalance,

                            ConsumedBalance = 0,

                            RemainingBalance = defaultBalance,

                            FiscalYearId = fiscalYear.Id
                        });
                }
            }





            if (leaveBalances.Any())
            {
                await _leaveBalanceRepository
                    .AddRangeAsync(
                        leaveBalances);



                await _leaveBalanceRepository
                    .SaveChangesAsync();
            }
        }





        // ==========================================
        // Generate Next Fiscal Year Automatically
        // ==========================================

        public async Task<FiscalYearResponseDto>
            GenerateNextAsync()
        {
            var activeFiscalYear =
                await _repository.GetActiveAsync();



            if (activeFiscalYear == null)
            {
                throw new BadRequestException(
                    "No active fiscal year exists.");
            }




            var settings =
                await _repository
                .GetFiscalYearSettingsAsync();



            if (settings == null)
            {
                throw new NotFoundException(
                    "Fiscal year settings not configured.");
            }




            // Calculate next fiscal year dates

            var nextStartYear =
                activeFiscalYear.EndDate.Year + 1;




            var startDate =
                new DateTime(
                    nextStartYear,
                    settings.StartMonth,
                    settings.StartDay);




            var endDate =
                startDate
                .AddYears(1)
                .AddDays(-1);




            var fiscalYearName =
                $"FY-{endDate.Year}";




            bool exists =
                await _repository
                .NameExistsAsync(
                    fiscalYearName);



            if (exists)
            {
                throw new BadRequestException(
                    "Next fiscal year already exists.");
            }




            var fiscalYear =
                new FiscalYear
                {
                    Name = fiscalYearName,

                    StartDate = startDate,

                    EndDate = endDate,

                    IsActive = false,

                    CreatedAt = DateTime.UtcNow
                };




            await _repository
                .AddAsync(
                    fiscalYear);



            await _repository
                .SaveChangesAsync();




            return new FiscalYearResponseDto
            {
                Id = fiscalYear.Id,

                Name = fiscalYear.Name,

                StartDate = fiscalYear.StartDate,

                EndDate = fiscalYear.EndDate,

                IsActive = fiscalYear.IsActive,

                CreatedAt = fiscalYear.CreatedAt
            };
        }
    }
}