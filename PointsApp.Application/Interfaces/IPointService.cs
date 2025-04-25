using PointsApp.Application.DTOs;

namespace PointsApp.Application.Interfaces
{
    public interface IPointService
    {
        Task<PointDto?> GetByIdAsync(int id);
        Task<List<PointDto>> GetAllAsync();
        Task<PointDto> AddAsync(PointDto pointDto);
        Task UpdateAsync(PointDto pointDto);
        Task DeleteAsync(int id);
        Task MoveAsync(int id, int x, int y);
    }
}
