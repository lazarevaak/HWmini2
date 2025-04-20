using System;
namespace ZooApplication.DTOs
{
    public sealed class FeedingRequest
    {
        public Guid AnimalId { get; }
        public DateTime FeedingTime { get; }
        public string FoodType { get; }

        public FeedingRequest(Guid animalId, DateTime feedingTime, string foodType)
        {
            AnimalId = animalId;
            FeedingTime = feedingTime;
            FoodType = foodType;
        }
    }
}

