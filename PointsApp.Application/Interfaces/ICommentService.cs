using PointsApp.Application.DTOs;

namespace PointsApp.Application.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetAllByPointIdAsync(int pointId);
        Task<CommentDto> AddAsync(CommentDto commentDto);
        Task UpdateAsync(int id, CommentDto commentDto);
        Task DeleteAsync(int id);
    }
}
