using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using LeaveManagement.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Features.FiscalYears.Repositories
{
    public class FiscalYearSettingsRepository
        : IFiscalYearSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public FiscalYearSettingsRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // Get Fiscal Year Settings
        // ==========================================

        public async Task<FiscalYearSetting?>
            GetAsync()
        {
            return await _context.FiscalYearSettings
                .FirstOrDefaultAsync();
        }

        // ==========================================
        // Add Fiscal Year Settings
        // ==========================================

        public async Task AddAsync(
            FiscalYearSetting settings)
        {
            await _context.FiscalYearSettings
                .AddAsync(settings);
        }

        // ==========================================
        // Save Changes
        // ==========================================

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}