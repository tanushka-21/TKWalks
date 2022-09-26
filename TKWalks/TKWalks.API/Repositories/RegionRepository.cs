using Microsoft.EntityFrameworkCore;
using TKWalks.API.Data;
using TKWalks.API.Models.Domain;

namespace TKWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly TKWalksDbContext tKWalksDbContext;

        public RegionRepository(TKWalksDbContext tKWalksDbContext)
        {
            this.tKWalksDbContext = tKWalksDbContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await tKWalksDbContext.Regions.ToListAsync();
        }
    }
}
