using System;
using ZooDomain.Entities;

namespace ZooApplication.Interfaces
{
    public interface IFeedingScheduleRepository
    {
        Task<FeedingSchedule> GetByIdAsync(Guid id);
        Task<IEnumerable<FeedingSchedule>> GetAllAsync();
        Task AddAsync(FeedingSchedule schedule);
        Task UpdateAsync(FeedingSchedule schedule);
        Task RemoveAsync(FeedingSchedule schedule);
    }
}

