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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await tKWalksDbContext.AddAsync(region);
            await tKWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await tKWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
                return null;
            tKWalksDbContext.Regions.Remove(region);
            await tKWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await tKWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            var region= await tKWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return region;
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await tKWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (existingRegion == null)
                return null;
            existingRegion.Code=region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await tKWalksDbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
