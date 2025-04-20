using System;
using Microsoft.Extensions.Options;
using Xunit;
using ZooApplication.DTOs;
using ZooApplication.Services;
using ZooDomain;
using ZooDomain.Actions;
using ZooDomain.Entities;
using ZooDomain.Enum;
using ZooInfrastructure.Repositories;
using System.Linq;

namespace ZooTest
{
    public class FeedingOrganizationServiceTests
    {
        private readonly InMemoryAnimalRepository _animals = new();
        private readonly InMemoryFeedingScheduleRepository _schedules = new();
        private readonly FeedingOrganizationService _service;

        public FeedingOrganizationServiceTests()
        {
            _service = new FeedingOrganizationService(_schedules, _animals);
        }

        [Fact]
        public async Task ScheduleFeedingAsync_Should_Add_Schedule()
        {
            // Arrange
            var animal = new Animal("Bear", "Baloo", DateTime.UtcNow.AddYears(-5), new Gender(), new Food("Berries", 200));
            await _animals.AddAsync(animal);
            var feedTime = DateTime.UtcNow.AddHours(2);
            var request = new FeedingRequest(animal.Id, feedTime, "Berries");

            // Act
            await _service.ScheduleFeedingAsync(request);

            // Assert
            // Преобразуем в List<FeedingSchedule>, чтобы индексатор стал доступен
            var list = (await _schedules.GetAllAsync()).ToList();
            Assert.Single(list);
            Assert.Equal(feedTime, list[0].FeedingTime);
            Assert.False(list[0].IsCompleted);
        }

        [Fact]
        public async Task ScheduleFeedingAsync_AnimalNotFound_ShouldThrow()
        {
            var request = new FeedingRequest(Guid.NewGuid(), DateTime.UtcNow.AddHours(1), "Food");
            await Assert.ThrowsAsync<Excep>(() => _service.ScheduleFeedingAsync(request));
        }

        [Fact]
        public async Task MarkFeedingCompletedAsync_Should_Mark_And_Raise_Event()
        {
            // Arrange
            var animal = new Animal("Elephant", "Ella", DateTime.UtcNow.AddYears(-10), new Gender(), new Food("Hay", 1000));
            await _animals.AddAsync(animal);

            var schedule = new FeedingSchedule(animal.Id, DateTime.UtcNow.AddMinutes(30), "Hay");
            await _schedules.AddAsync(schedule);

            // Act
            await _service.MarkFeedingCompletedAsync(schedule.Id);

            // Assert
            // здесь можем снова преобразовать или использовать First()
            var updated = (await _schedules.GetAllAsync()).First(s => s.Id == schedule.Id);
            Assert.True(updated.IsCompleted);
            Assert.Contains(updated.DomainEvents, e => e is FeedingTimeEvent);
        }

        [Fact]
        public async Task MarkFeedingCompletedAsync_ScheduleNotFound_ShouldThrow()
        {
            await Assert.ThrowsAsync<Excep>(() => _service.MarkFeedingCompletedAsync(Guid.NewGuid()));
        }
    }
}

