using System;
using Xunit;
using ZooDomain;
using ZooDomain.Entities;
using ZooDomain.Enum;

namespace ZooTest
{
    public class AnimalTests
    {
        [Fact]
        public void Ctor_Should_Initialize_Properties()
        {
            var dob = new DateTime(2020, 1, 1);
            var food = new Food("Hay", 50);
            var gender = new Gender();

            var a = new Animal("Horse", "Harry", dob, gender, food);

            Assert.Equal("Horse", a.Species);
            Assert.Equal("Harry", a.Name);
            Assert.Equal(dob, a.DateOfBirth);
            Assert.Equal(gender, a.Gender);
            Assert.Equal(food, a.FavoriteFood);
            Assert.Equal(HealthStatus.Healthy, a.Status);
            Assert.Null(a.EnclosureId);
        }

        [Theory]
        [InlineData("", "Name")]
        [InlineData("Species", "")]
        public void Ctor_InvalidStrings_ShouldThrow(string sp, string nm)
        {
            var dob = DateTime.UtcNow;
            var gender = new Gender();
            var food = new Food("X", 10);

            Assert.Throws<Excep>(() =>
                new Animal(sp, nm, dob, gender, food));
        }

        [Fact]
        public void Feed_WrongFood_ShouldThrow()
        {
            var animal = new Animal("Cow", "Cowie", DateTime.UtcNow.AddYears(-1),
                new Gender(), new Food("Grass", 100));
            var wrong = new Food("Hay", 50);

            var ex = Assert.Throws<Excep>(() => animal.Feed(wrong));
            Assert.Contains("не любимая", ex.Message);
        }

        [Fact]
        public void Feed_WhenSick_ShouldThrow()
        {
            var animal = new Animal("Cow", "Cowie", DateTime.UtcNow.AddYears(-1),
                new Gender(), new Food("Grass", 100));
            animal.MarkSick();
            Assert.Throws<Excep>(() => animal.Feed(new Food("Grass", 100)));
        }

        [Fact]
        public void Heal_And_MarkSick_Should_UpdateStatus()
        {
            var animal = new Animal("Cow", "Cowie", DateTime.UtcNow.AddYears(-1),
                new Gender(), new Food("Grass", 100));

            animal.MarkSick();
            Assert.Equal(HealthStatus.Sick, animal.Status);

            animal.Heal();
            Assert.Equal(HealthStatus.Healthy, animal.Status);
        }

        [Fact]
        public void MoveTo_Should_Raise_Event_And_SetEnclosure()
        {
            var animal = new Animal("Goat", "Gogo", DateTime.UtcNow.AddYears(-2),
                new Gender(), new Food("Leaves", 30));

            var newId = Guid.NewGuid();
            animal.MoveTo(newId);

            Assert.Equal(newId, animal.EnclosureId);
            var ev = Assert.Single(animal.DomainEvents.OfType<AnimalMovedEvent>());
            Assert.Equal(newId, ev.ToEnclosureId);
        }

        [Fact]
        public void MoveTo_SameEnclosure_ShouldThrow()
        {
            var id = Guid.NewGuid();
            var animal = new Animal("Goat", "Gogo", DateTime.UtcNow.AddYears(-2),
                new Gender(), new Food("Leaves", 30));
            animal.MoveTo(id);

            Assert.Throws<Excep>(() => animal.MoveTo(id));
        }
    }
}

