using Microsoft.EntityFrameworkCore;
using PointsApp.Domain.Entities;
using PointsApp.Domain.Interfaces;

namespace PointsApp.Infrastructure.Repositories
{
    public  class CommentsRepository : ICommentsRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Comment?> GetByIdAsync(int id)
            => await _appDbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        public async Task<List<Comment>> GetAllByPointIdAsync(int pointId)
            => await _appDbContext.Comments.Where(comment => comment.PointId == pointId).ToListAsync();

        public async Task<Comment> AddAsync(Comment comment)
        {
            try
            {
                await _appDbContext.AddAsync(comment);
                await _appDbContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении комментария: " + ex.Message);
                throw;
            }

            return await _appDbContext.Comments.LastAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _appDbContext.Update(comment);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            if(comment != null)
            {
                _appDbContext.Remove(comment);
                await _appDbContext.SaveChangesAsync();
            }    
        }
    }
}
