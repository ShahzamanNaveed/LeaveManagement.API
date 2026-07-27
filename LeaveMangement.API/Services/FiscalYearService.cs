using LeaveManagement.API.Exceptions;
using LeaveManagement.API.Models;
using LeaveMangement.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.API.Data;


namespace LeaveMangement.API.Services
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