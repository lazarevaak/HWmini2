using System;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;

namespace ZooApplication.Services
{
    public class ZooStatisticsService : IZooStatisticsService
    {
        private readonly IAnimalRepository _animals;
        private readonly IEnclosureRepository _enclosures;
        private readonly IFeedingScheduleRepository _schedules;

        public ZooStatisticsService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository,
            IFeedingScheduleRepository scheduleRepository)
        {
            _animals = animalRepository;
            _enclosures = enclosureRepository;
            _schedules = scheduleRepository;
        }

        public async Task<ZooStatisticsDto> GetStatisticsAsync()
        {
            var allAnimals = (await _animals.GetAllAsync()).ToList();
            var allEnclosures = (await _enclosures.GetAllAsync()).ToList();
            var allSchedules = (await _schedules.GetAllAsync()).ToList();

            var freeEnclosures = allEnclosures.Count(e => e.AnimalIds.Count < e.Capacity);
            var upcomingFeedings = allSchedules
                .Count(s => !s.IsCompleted && s.FeedingTime > DateTime.UtcNow);

            return new ZooStatisticsDto
            {
                TotalAnimals = allAnimals.Count,
                TotalEnclosures = allEnclosures.Count,
                FreeEnclosures = freeEnclosures,
                UpcomingFeedings = upcomingFeedings
            };
        }
    }
}

