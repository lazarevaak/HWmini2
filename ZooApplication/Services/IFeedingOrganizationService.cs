using System;
using ZooApplication.DTOs;

namespace ZooApplication.Services
{
    public interface IFeedingOrganizationService
    {
        Task ScheduleFeedingAsync(FeedingRequest request);
        Task MarkFeedingCompletedAsync(Guid feedingScheduleId);
    }
}

