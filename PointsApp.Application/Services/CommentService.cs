using PointsApp.Application.DTOs;
using PointsApp.Application.Interfaces;
using PointsApp.Domain.Interfaces;
using PointsApp.Application.Helpers;

namespace PointsApp.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsRepository _commentsRepository;

        public CommentService(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<List<CommentDto>> GetAllByPointIdAsync(int pointId)
        {
            var comments = await _commentsRepository.GetAllByPointIdAsync(pointId);
            var commentsDto = comments.ToCommentsDto();

            return commentsDto;
        }

        public async Task<CommentDto> AddAsync(CommentDto commentDto)
        {
            var comment = commentDto.ToComment();
            comment = await _commentsRepository.AddAsync(comment);
            commentDto = comment.ToCommentDto();

            return commentDto;
        }

        public async Task UpdateAsync(int id, CommentDto commentDto)
        {
            var existingComment = await _commentsRepository.GetByIdAsync(id);
            if (existingComment == null)
                throw new KeyNotFoundException($"Comment id: {id} is not found");

            existingComment.Text = commentDto.Text;
            existingComment.BackgroundColor = commentDto.BackgroundColor;

            await _commentsRepository.UpdateAsync(existingComment);
        }

        public async Task DeleteAsync(int id)
        {
            await _commentsRepository.DeleteAsync(id);
        }
    }
}
