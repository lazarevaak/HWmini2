using System;
namespace ZooDomain
{
    public sealed record AnimalMovedEvent(
       Guid AnimalId,
       Guid FromEnclosureId,
       Guid ToEnclosureId,
       DateTime OccurredAt
   ) : IDomainEvent
    {
        public AnimalMovedEvent(
            Guid animalId,
            Guid fromEnclosureId,
            Guid toEnclosureId
        ) : this(animalId, fromEnclosureId, toEnclosureId, DateTime.UtcNow)
        { }
    }
}

