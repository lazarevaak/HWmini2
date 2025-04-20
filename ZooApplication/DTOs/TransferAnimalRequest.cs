using System;
namespace ZooApplication.DTOs
{
    public sealed class TransferAnimalRequest
    {
        public Guid AnimalId { get; }
        public Guid TargetEnclosureId { get; }

        public TransferAnimalRequest(Guid animalId, Guid targetEnclosureId)
        {
            AnimalId = animalId;
            TargetEnclosureId = targetEnclosureId;
        }
    }
}

