using System;
using System.Collections.Concurrent;
using ZooApplication.Interfaces;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories
{
    public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly ConcurrentDictionary<Guid, FeedingSchedule> _storage = new();

        public Task AddAsync(FeedingSchedule schedule)
        {
            if (!_storage.TryAdd(schedule.Id, schedule))
                throw new InvalidOperationException($"FeedingSchedule with Id {schedule.Id} already exists.");
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<FeedingSchedule>>(_storage.Values.ToList());
        }

        public Task<FeedingSchedule> GetByIdAsync(Guid id)
        {
            _storage.TryGetValue(id, out var schedule);
            return Task.FromResult(schedule);
        }

        public Task RemoveAsync(FeedingSchedule schedule)
        {
            _storage.TryRemove(schedule.Id, out _);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(FeedingSchedule schedule)
        {
            _storage[schedule.Id] = schedule;
            return Task.CompletedTask;
        }
    }
}

