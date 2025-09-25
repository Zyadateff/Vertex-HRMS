using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class RevenueRepo:IRevenueRepo
    {
        private readonly VertexHRMSDbContext _context;
        public RevenueRepo(VertexHRMSDbContext db)
        {
            _context = db;
        }
        public async Task<IEnumerable<Revenue>> GetAllAsync()
        {
            return await _context.Revenues
                .OrderBy(r => r.MonthYear)
                .ToListAsync();
        }

        public async Task<Revenue?> GetByIdAsync(int id)
        {
            return await _context.Revenues.FindAsync(id);
        }

        public async Task AddAsync(Revenue revenue)
        {
            await _context.Revenues.AddAsync(revenue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Revenue revenue)
        {
            _context.Revenues.Update(revenue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var revenue = await _context.Revenues.FindAsync(id);
            if (revenue != null)
            {
                _context.Revenues.Remove(revenue);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<int, IEnumerable<Revenue>>> GetQuarterlyAsync(int year)
        {
            var revenues = await _context.Revenues
                .Where(r => r.MonthYear.Year == year)
                .OrderBy(r => r.MonthYear)
                .ToListAsync();

            return revenues
                .GroupBy(r => ((r.MonthYear.Month - 1) / 3) + 1)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());
        }

        public async Task<IEnumerable<Revenue>> GetQuarterAsync(int year, int quarter)
        {
            return await _context.Revenues
                .Where(r => r.MonthYear.Year == year &&
                            ((r.MonthYear.Month - 1) / 3) + 1 == quarter)
                .OrderBy(r => r.MonthYear)
                .ToListAsync();
        }
    }
}

