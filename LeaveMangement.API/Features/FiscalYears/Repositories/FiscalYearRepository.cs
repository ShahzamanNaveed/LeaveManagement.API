using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using LeaveManagement.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Features.FiscalYears.Repositories
{
    public class FiscalYearRepository
        : IFiscalYearRepository
    {
        private readonly ApplicationDbContext _context;


        public FiscalYearRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }



        // ==========================================
        // Get All Fiscal Years
        // ==========================================

        public async Task<List<FiscalYear>>
            GetAllAsync()
        {
            return await _context.FiscalYears

                .OrderByDescending(
                    x => x.StartDate)

                .ToListAsync();
        }




        // ==========================================
        // Get Fiscal Year By Id
        // ==========================================

        public async Task<FiscalYear?>
            GetByIdAsync(
                int id)
        {
            return await _context.FiscalYears

                .FirstOrDefaultAsync(
                    x => x.Id == id);
        }




        // ==========================================
        // Get Active Fiscal Year
        // ==========================================

        public async Task<FiscalYear?>
            GetActiveAsync()
        {
            return await _context.FiscalYears

                .FirstOrDefaultAsync(
                    x => x.IsActive);
        }




        // ==========================================
        // Get Fiscal Year Settings
        // ==========================================

        public async Task<FiscalYearSetting?>
            GetFiscalYearSettingsAsync()
        {
            return await _context.FiscalYearSettings

                .FirstOrDefaultAsync();
        }




        // ==========================================
        // Check Duplicate Name
        // ==========================================

        public async Task<bool>
            NameExistsAsync(
                string name)
        {
            return await _context.FiscalYears

                .AnyAsync(x =>
                    x.Name == name);
        }




        // ==========================================
        // Check Date Overlap
        // ==========================================

        public async Task<bool>
            HasOverlappingDatesAsync(
                DateTime startDate,
                DateTime endDate)
        {
            return await _context.FiscalYears

                .AnyAsync(x =>
                    startDate <= x.EndDate &&
                    endDate >= x.StartDate);
        }




        // ==========================================
        // Add Fiscal Year
        // ==========================================

        public async Task AddAsync(
            FiscalYear fiscalYear)
        {
            await _context.FiscalYears

                .AddAsync(fiscalYear);
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