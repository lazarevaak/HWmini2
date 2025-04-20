using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooApplication.Interfaces;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories
{
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly ConcurrentDictionary<Guid, Animal> _storage = new();

        public Task AddAsync(Animal animal)
        {
            if (!_storage.TryAdd(animal.Id, animal))
                throw new InvalidOperationException($"Animal with Id {animal.Id} already exists.");
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Animal>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Animal>>(_storage.Values.ToList());
        }

        public Task<Animal> GetByIdAsync(Guid id)
        {
            _storage.TryGetValue(id, out var animal);
            return Task.FromResult(animal);
        }

        public Task RemoveAsync(Animal animal)
        {
            _storage.TryRemove(animal.Id, out _);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Animal animal)
        {
            _storage[animal.Id] = animal;
            return Task.CompletedTask;
        }
    }
}

