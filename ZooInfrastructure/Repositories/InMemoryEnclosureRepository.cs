using System;
using System.Collections.Concurrent;
using ZooApplication.Interfaces;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories
{
    public class InMemoryEnclosureRepository : IEnclosureRepository
    {
        private readonly ConcurrentDictionary<Guid, Enclosure> _storage = new();

        public Task AddAsync(Enclosure enclosure)
        {
            if (!_storage.TryAdd(enclosure.Id, enclosure))
                throw new InvalidOperationException($"Enclosure with Id {enclosure.Id} already exists.");
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Enclosure>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Enclosure>>(_storage.Values.ToList());
        }

        public Task<Enclosure> GetByIdAsync(Guid id)
        {
            _storage.TryGetValue(id, out var enclosure);
            return Task.FromResult(enclosure);
        }

        public Task RemoveAsync(Enclosure enclosure)
        {
            _storage.TryRemove(enclosure.Id, out _);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Enclosure enclosure)
        {
            _storage[enclosure.Id] = enclosure;
            return Task.CompletedTask;
        }
    }
}

