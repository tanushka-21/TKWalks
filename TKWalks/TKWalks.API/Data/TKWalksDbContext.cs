using Microsoft.EntityFrameworkCore;
using TKWalks.API.Models.Domain;

namespace TKWalks.API.Data
{
    public class TKWalksDbContext : DbContext
    {
        public TKWalksDbContext(DbContextOptions<TKWalksDbContext> options) : base(options)
        {

        }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
