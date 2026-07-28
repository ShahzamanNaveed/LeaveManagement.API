using LeaveManagement.API.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Infrastructure.Persistence;


namespace LeaveManagement.API.Features.FiscalYears.Services
{
    public class FiscalYearService : IFiscalYearService
    {

        private readonly ApplicationDbContext _context;



        public FiscalYearService(
            ApplicationDbContext context)
        {
            _context = context;
        }




        public async Task<FiscalYear>
            GetActiveFiscalYearAsync()
        {

            var fiscalYear =
                await _context.FiscalYears
                .FirstOrDefaultAsync(
                    f => f.IsActive);



            if (fiscalYear == null)
            {
                throw new NotFoundException(
                    "Active fiscal year not found.");
            }



            return fiscalYear;

        }

    }
}