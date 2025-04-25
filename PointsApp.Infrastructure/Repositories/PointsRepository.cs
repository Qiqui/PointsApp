using Microsoft.EntityFrameworkCore;
using PointsApp.Domain.Entities;
using PointsApp.Domain.Interfaces;

namespace PointsApp.Infrastructure.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly AppDbContext _appDbContext;

        public PointsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Point?> GetByIdAsync(int id)
            => await _appDbContext.Points.Include(point => point.Comments)
                                         .FirstOrDefaultAsync(point => point.Id == id);

        public async Task<List<Point>> GetAllAsync()
            => await _appDbContext.Points.Include(point => point.Comments)
                                         .ToListAsync();

        public async Task<Point> AddAsync(Point point)
        {
           var entry =  await _appDbContext.AddAsync(point);
            await _appDbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task UpdateAsync(Point point)
        {
            _appDbContext.Points.Update(point);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var point = await GetByIdAsync(id);
            if(point != null)
            {
                _appDbContext.Points.Remove(point);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
