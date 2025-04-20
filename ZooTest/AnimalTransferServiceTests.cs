using System;
using Xunit;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;
using ZooApplication.Services;
using ZooDomain;
using ZooDomain.Entities;
using ZooDomain.Enum;
using ZooInfrastructure.Repositories;

namespace ZooTest
{
    public class AnimalTransferServiceTests
    {
        private readonly InMemoryAnimalRepository _animals = new();
        private readonly InMemoryEnclosureRepository _enclosures = new();
        private readonly AnimalTransferService _service;

        public AnimalTransferServiceTests()
        {
            _service = new AnimalTransferService(_animals, _enclosures);
        }

        [Fact]
        public async Task TransferAsync_Should_Move_Animal_Between_Enclosures()
        {
            // Arrange: создаём два вольера
            var e1 = new Enclosure(EnclosureType.Herbivore, 50, 2);
            var e2 = new Enclosure(EnclosureType.Herbivore, 60, 2);
            await _enclosures.AddAsync(e1);
            await _enclosures.AddAsync(e2);

            // Создаём животное и вручную добавляем его в e1
            var animal = new Animal("Deer", "Bambi", DateTime.UtcNow.AddYears(-1), new Gender(), new Food("Grass", 100));
            animal.MoveTo(e1.Id);
            e1.AddAnimal(animal);
            await _animals.AddAsync(animal);
            await _enclosures.UpdateAsync(e1);

            var request = new TransferAnimalRequest(animal.Id, e2.Id);

            // Act
            await _service.TransferAsync(request);

            // Assert: животное перекочевало…
            var a = await _animals.GetByIdAsync(animal.Id);
            Assert.Equal(e2.Id, a.EnclosureId);

            // …и ушло из старого…
            var old = await _enclosures.GetByIdAsync(e1.Id);
            Assert.DoesNotContain(animal.Id, old.AnimalIds);

            // …и появилось в новом
            var target = await _enclosures.GetByIdAsync(e2.Id);
            Assert.Contains(animal.Id, target.AnimalIds);
        }

        [Fact]
        public async Task TransferAsync_TargetFull_ShouldThrow()
        {
            // Arrange: полон вольер
            var full = new Enclosure(EnclosureType.Predator, 30, 1);
            await _enclosures.AddAsync(full);

            var a1 = new Animal("Lion", "Leo", DateTime.UtcNow.AddYears(-3), new Gender(), new Food("Meat", 500));
            a1.MoveTo(full.Id);
            full.AddAnimal(a1);
            await _animals.AddAsync(a1);
            await _enclosures.UpdateAsync(full);

            var a2 = new Animal("Tiger", "Tig", DateTime.UtcNow.AddYears(-2), new Gender(), new Food("Meat", 400));
            await _animals.AddAsync(a2);

            var request = new TransferAnimalRequest(a2.Id, full.Id);

            // Act & Assert: переполнено
            var ex = await Assert.ThrowsAsync<Excep>(() => _service.TransferAsync(request));
            Assert.Contains("переполнен", ex.Message);
        }
    }
}

