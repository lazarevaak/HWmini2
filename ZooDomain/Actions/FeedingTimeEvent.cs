using System;
namespace ZooDomain.Actions
{
    public sealed record FeedingTimeEvent(
        Guid AnimalId,
        DateTime ScheduledAt,
        string FoodType,
        DateTime OccurredAt
    ) : IDomainEvent
    {
        public FeedingTimeEvent(Guid animalId, DateTime scheduledAt, string foodType)
            : this(animalId, scheduledAt, foodType, DateTime.UtcNow) { }
    }
}

