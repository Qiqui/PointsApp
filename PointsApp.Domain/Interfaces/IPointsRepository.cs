using PointsApp.Domain.Entities;

namespace PointsApp.Domain.Interfaces
{
    public interface IPointsRepository
    {
        Task<Point?> GetByIdAsync(int id);
        Task<List<Point>> GetAllAsync();
        Task<Point> AddAsync(Point point);
        Task UpdateAsync(Point point);
        Task DeleteAsync(int id);
    }
}