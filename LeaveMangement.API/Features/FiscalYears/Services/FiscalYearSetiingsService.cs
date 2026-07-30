using LeaveManagement.API.Common.Exceptions;
using LeaveManagement.API.Features.FiscalYears.Dtos;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using LeaveManagement.API.Domain.Entities;

namespace LeaveManagement.API.Features.FiscalYears.Services
{
    public class FiscalYearSettingsService
        : IFiscalYearSettingsService
    {
        private readonly IFiscalYearSettingsRepository _repository;

        public FiscalYearSettingsService(
            IFiscalYearSettingsRepository repository)
        {
            _repository = repository;
        }

        // ==========================================
        // Get Settings
        // ==========================================

        public async Task<FiscalYearSettingsResponseDto>
            GetAsync()
        {
            var settings =
                await _repository.GetAsync();

            if (settings == null)
            {
                throw new NotFoundException(
                    "Fiscal year settings not found.");
            }

            return new FiscalYearSettingsResponseDto
            {
                Id = settings.Id,
                StartMonth = settings.StartMonth,
                StartDay = settings.StartDay,
                CreatedAt = settings.CreatedAt
            };
        }

        // ==========================================
        // Create Settings
        // ==========================================

        public async Task<FiscalYearSettingsResponseDto>
            CreateAsync(
                CreateFiscalYearSettingsDto request)
        {
            // Only one settings record is allowed

            var existing =
                await _repository.GetAsync();

            if (existing != null)
            {
                throw new BadRequestException(
                    "Fiscal year settings already exist.");
            }

            ValidateDate(request.StartMonth, request.StartDay);

            var settings =
                new FiscalYearSetting
                {
                    StartMonth = request.StartMonth,
                    StartDay = request.StartDay,
                    CreatedAt = DateTime.UtcNow
                };

            await _repository.AddAsync(settings);

            await _repository.SaveChangesAsync();

            return new FiscalYearSettingsResponseDto
            {
                Id = settings.Id,
                StartMonth = settings.StartMonth,
                StartDay = settings.StartDay,
                CreatedAt = settings.CreatedAt
            };
        }

        // ==========================================
        // Update Settings
        // ==========================================

        public async Task<FiscalYearSettingsResponseDto>
            UpdateAsync(
                CreateFiscalYearSettingsDto request)
        {
            var settings =
                await _repository.GetAsync();

            if (settings == null)
            {
                throw new NotFoundException(
                    "Fiscal year settings not found.");
            }

            ValidateDate(request.StartMonth, request.StartDay);

            settings.StartMonth = request.StartMonth;
            settings.StartDay = request.StartDay;

            await _repository.SaveChangesAsync();

            return new FiscalYearSettingsResponseDto
            {
                Id = settings.Id,
                StartMonth = settings.StartMonth,
                StartDay = settings.StartDay,
                CreatedAt = settings.CreatedAt
            };
        }

        // ==========================================
        // Validation
        // ==========================================

        private static void ValidateDate(
    int month,
    int day)
        {
            try
            {
                _ = new DateTime(2024, month, day);
            }
            catch
            {
                throw new BadRequestException(
                    "Invalid start day for the selected month.");
            }
        }
    }
}