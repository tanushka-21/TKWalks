using Microsoft.EntityFrameworkCore;
using TKWalks.API.Data;
using TKWalks.API.Models.Domain;

namespace TKWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly TKWalksDbContext tKWalksDbContext;

        public WalkRepository(TKWalksDbContext tKWalksDbContext)
        {
            this.tKWalksDbContext = tKWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await tKWalksDbContext.Walks.AddAsync(walk);
            await tKWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await tKWalksDbContext.Walks.FindAsync(id);
            if (existingWalk == null)
                return null;
            tKWalksDbContext.Walks.Remove(existingWalk);
            await tKWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await tKWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await tKWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await tKWalksDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await tKWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return null;
            
        }
    }
}
