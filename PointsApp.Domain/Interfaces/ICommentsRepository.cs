using PointsApp.Domain.Entities;

namespace PointsApp.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<List<Comment>> GetAllByPointIdAsync(int PointId);
        Task<Comment> AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
    }
}
