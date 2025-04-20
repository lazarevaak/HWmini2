using System;
namespace ZooDomain
{
    public sealed record Food
    {
        public string Name { get; }
        public int Calories { get; }

        public Food(string name, int calories)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Excep("Название еды не может быть пустым.");
            if (calories <= 0)
                throw new Excep("Калорийность должна быть положительной.");

            Name = name;
            Calories = calories;
        }

        public override string ToString() => $"{Name} ({Calories} ккал)";
    }
}

