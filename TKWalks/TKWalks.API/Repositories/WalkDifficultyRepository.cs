using Microsoft.EntityFrameworkCore;
using TKWalks.API.Data;
using TKWalks.API.Models.Domain;

namespace TKWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly TKWalksDbContext tKWalksDbContext;

        public WalkDifficultyRepository(TKWalksDbContext tKWalksDbContext)
        {
            this.tKWalksDbContext = tKWalksDbContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await tKWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await tKWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDiff = await tKWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDiff == null)
                return null;
            tKWalksDbContext.WalkDifficulty.Remove(existingWalkDiff);
            await tKWalksDbContext.SaveChangesAsync();
            return existingWalkDiff;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await tKWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await tKWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDiff = await tKWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDiff == null)
                return null;
            existingWalkDiff.Code = walkDifficulty.Code;
            await tKWalksDbContext.SaveChangesAsync();
            return existingWalkDiff;
        }
    }
}
