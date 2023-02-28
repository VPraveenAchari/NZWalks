using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.IRepository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region> GetAsync(Guid id);
        Task<Region> AddAsync(Region region);
        Task<Region> DeleteAsync(Guid id);
        Task<Region> UpdateAsync(Guid id, Region region);
    }
}
