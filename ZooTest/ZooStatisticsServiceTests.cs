using System;
using Xunit;
using ZooApplication.Services;
using ZooDomain;
using ZooDomain.Entities;
using ZooDomain.Enum;
using ZooInfrastructure.Repositories;

namespace ZooTest
{
    public class ZooStatisticsServiceTests
    {
        private readonly InMemoryAnimalRepository _animals = new();
        private readonly InMemoryEnclosureRepository _enclosures = new();
        private readonly InMemoryFeedingScheduleRepository _schedules = new();
        private readonly ZooStatisticsService _service;

        public ZooStatisticsServiceTests()
        {
            _service = new ZooStatisticsService(_animals, _enclosures, _schedules);
        }

        [Fact]
        public async Task GetStatisticsAsync_Returns_Correct_Values()
        {
            // Arrange: создаём вольеры
            var e1 = new Enclosure(EnclosureType.Predator, 100, 2);
            var e2 = new Enclosure(EnclosureType.Herbivore, 80, 1);
            await _enclosures.AddAsync(e1);
            await _enclosures.AddAsync(e2);

            // два животного в e1
            var a1 = new Animal("Lion", "Leo", DateTime.UtcNow.AddYears(-3), new Gender(), new Food("Meat", 400));
            var a2 = new Animal("Gazelle", "Gigi", DateTime.UtcNow.AddYears(-2), new Gender(), new Food("Grass", 150));
            a1.MoveTo(e1.Id); a2.MoveTo(e1.Id);
            e1.AddAnimal(a1); e1.AddAnimal(a2);
            await _animals.AddAsync(a1);
            await _animals.AddAsync(a2);
            await _enclosures.UpdateAsync(e1);

            // одно грядущее кормление (future) и одно завершённое
            var future = new FeedingSchedule(a1.Id, DateTime.UtcNow.AddHours(1), "Meat");
            await _schedules.AddAsync(future);
            var completed = new FeedingSchedule(a2.Id, DateTime.UtcNow.AddHours(2), "Grass");
            await _schedules.AddAsync(completed);
            completed.MarkCompleted();
            await _schedules.UpdateAsync(completed);

            // Act
            var stats = await _service.GetStatisticsAsync();

            // Assert
            Assert.Equal(2, stats.TotalAnimals);
            Assert.Equal(2, stats.TotalEnclosures);
            Assert.Equal(1, stats.FreeEnclosures);    // e2 пуст
            Assert.Equal(1, stats.UpcomingFeedings);  // только future не completed
        }
    }
}

