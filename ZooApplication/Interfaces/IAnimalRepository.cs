using System;
using ZooDomain.Entities;

namespace ZooApplication.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetByIdAsync(Guid id);
        Task<IEnumerable<Animal>> GetAllAsync();
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task RemoveAsync(Animal animal);
    }
}

