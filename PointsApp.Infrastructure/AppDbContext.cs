using Microsoft.EntityFrameworkCore;
using PointsApp.Domain.Entities;

namespace PointsApp.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Point> Points { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
