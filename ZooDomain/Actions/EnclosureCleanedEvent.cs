using System;
namespace ZooDomain.Actions
{
    public sealed record EnclosureCleanedEvent(Guid EnclosureId, DateTime CleanedAt) : IDomainEvent;
}

