using System;
using Xunit;
using ZooDomain;
using ZooDomain.Actions;
using ZooDomain.Entities;
using ZooDomain.Enum;

namespace ZooTest
{
    public class EnclosureTests
    {
        [Theory]
        [InlineData(0, 5)]
        [InlineData(5, 0)]
        [InlineData(-1, 5)]
        public void Ctor_InvalidArgs_ShouldThrow(double area, int cap)
        {
            Assert.Throws<Excep>(() =>
                new Enclosure(EnclosureType.Aviary, area, cap));
        }

        [Fact]
        public void AddAndRemove_Animal_Should_Work_And_RaiseDirty()
        {
            var e = new Enclosure(EnclosureType.Predator, 10, 2);
            var animal = new Animal("Lion", "Leo", DateTime.UtcNow.AddYears(-3),
                new Gender(), new Food("Meat", 300));

            e.AddAnimal(animal);
            Assert.Contains(animal.Id, e.AnimalIds);
            Assert.True(e.IsDirty);

            e.RemoveAnimal(animal);
            Assert.DoesNotContain(animal.Id, e.AnimalIds);
            Assert.True(e.IsDirty);
        }

        [Fact]
        public void AddAnimal_WhenFull_ShouldThrow()
        {
            var e = new Enclosure(EnclosureType.Predator, 10, 1);
            var a1 = new Animal("Tiger", "Tig", DateTime.UtcNow.AddYears(-2),
                new Gender(), new Food("Meat", 200));
            var a2 = new Animal("Tiger", "Tig2", DateTime.UtcNow.AddYears(-2),
                new Gender(), new Food("Meat", 200));

            e.AddAnimal(a1);
            Assert.Throws<Excep>(() => e.AddAnimal(a2));
        }

        [Fact]
        public void RemoveAnimal_NotPresent_ShouldThrow()
        {
            var e = new Enclosure(EnclosureType.Aviary, 5, 1);
            var a = new Animal("Parrot", "Polly", DateTime.UtcNow.AddYears(-1),
                new Gender(), new Food("Seeds", 20));
            Assert.Throws<Excep>(() => e.RemoveAnimal(a));
        }

        [Fact]
        public void Clean_WhenNotDirty_ShouldThrow_Then_Clean_Success()
        {
            var e = new Enclosure(EnclosureType.Herbivore, 10, 2);

            Assert.False(e.IsDirty);
            Assert.Throws<Excep>(() => e.Clean());

            // make dirty
            var a = new Animal("Deer", "Bambi", DateTime.UtcNow.AddYears(-1),
                new Gender(), new Food("Grass", 50));
            e.AddAnimal(a);

            e.Clean();
            Assert.False(e.IsDirty);
            Assert.NotNull(e.LastCleanedAt);

            var ev = Assert.Single(e.DomainEvents.OfType<EnclosureCleanedEvent>());
            Assert.Equal(e.Id, ev.EnclosureId);
        }
    }
}

