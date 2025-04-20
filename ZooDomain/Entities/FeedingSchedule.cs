using System;
using ZooDomain.Actions;

namespace ZooDomain.Entities
{
    public class FeedingSchedule : Entity
    {
        public Guid AnimalId { get; private set; }
        public DateTime FeedingTime { get; private set; }
        public string FoodType { get; private set; }
        public bool IsCompleted { get; private set; }

        private FeedingSchedule() { }

        public FeedingSchedule(
            Guid animalId,
            DateTime feedingTime,
            string foodType
        )
        {
            if (animalId == Guid.Empty)
                throw new Excep("Идентификатор животного обязателен.");
            if (feedingTime < DateTime.UtcNow)
                throw new Excep("Время кормления должно быть в будущем.");
            if (string.IsNullOrWhiteSpace(foodType))
                throw new Excep("Тип пищи не может быть пустым.");

            AnimalId = animalId;
            FeedingTime = feedingTime;
            FoodType = foodType;
            IsCompleted = false;
        }

        /// <summary>
        /// Изменить время кормления.
        /// </summary>
        public void Reschedule(DateTime newTime)
        {
            if (newTime < DateTime.UtcNow)
                throw new Excep("Новое время должно быть в будущем.");
            FeedingTime = newTime;
            IsCompleted = false;
        }

        /// <summary>
        /// Отметить выполнение кормления и поднять событие.
        /// </summary>
        public void MarkCompleted()
        {
            if (IsCompleted)
                throw new Excep("Кормление уже отмечено.");
            IsCompleted = true;

            AddDomainEvent(new FeedingTimeEvent(
                AnimalId,
                FeedingTime,
                FoodType
            ));
        }
    }
}

