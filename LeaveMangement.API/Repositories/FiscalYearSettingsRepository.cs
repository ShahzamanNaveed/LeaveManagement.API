using LeaveManagement.API.Data;
using LeaveManagement.API.Models;
using LeaveMangement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Repositories
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