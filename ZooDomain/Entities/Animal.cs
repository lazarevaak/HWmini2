using System;
using ZooDomain.Enum;

namespace ZooDomain.Entities
{
    public class Animal : Entity
    {
        public string Species { get; private set; }
        public string Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public Food FavoriteFood { get; private set; }
        public HealthStatus Status { get; private set; }
        public Guid? EnclosureId { get; private set; }

        private Animal() { }

        public Animal(
            string species,
            string name,
            DateTime dateOfBirth,
            Gender gender,
            Food favoriteFood
        )
        {
            if (string.IsNullOrWhiteSpace(species))
                throw new Excep("Вид не может быть пустым.");
            if (string.IsNullOrWhiteSpace(name))
                throw new Excep("Имя не может быть пустым.");
            Species = species;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            FavoriteFood = favoriteFood ?? throw new Excep("Укажите любимую еду.");
            Status = HealthStatus.Healthy;
        }

        public void Feed(Food food)
        {
            if (Status == HealthStatus.Sick)
                throw new Excep("Нельзя кормить больное животное.");
            if (food.Name != FavoriteFood.Name)
                throw new Excep(
                    $"Это не любимая еда («{FavoriteFood.Name}»)."
                );
        }

        public void Heal() => Status = HealthStatus.Healthy;
        public void MarkSick() => Status = HealthStatus.Sick;

        public void MoveTo(Guid newEnclosureId)
        {
            if (EnclosureId == newEnclosureId)
                throw new Excep("Животное уже в этом вольере.");

            var oldId = EnclosureId ?? Guid.Empty;
            EnclosureId = newEnclosureId;
            AddDomainEvent(new AnimalMovedEvent(
                Id,
                oldId,
                newEnclosureId
            ));
        }
    }
}

