using System;
using ZooDomain.Entities;

namespace ZooApplication.Interfaces
{
    public interface IEnclosureRepository
    {
        Task<Enclosure> GetByIdAsync(Guid id);
        Task<IEnumerable<Enclosure>> GetAllAsync();
        Task AddAsync(Enclosure enclosure);
        Task UpdateAsync(Enclosure enclosure);
        Task RemoveAsync(Enclosure enclosure);
    }
}

