using System;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;
using ZooDomain;
using ZooDomain.Entities;

namespace ZooApplication.Services
{
    public class FeedingOrganizationService : IFeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _schedules;
        private readonly IAnimalRepository _animals;

        public FeedingOrganizationService(
            IFeedingScheduleRepository scheduleRepository,
            IAnimalRepository animalRepository)
        {
            _schedules = scheduleRepository;
            _animals = animalRepository;
        }

        public async Task ScheduleFeedingAsync(FeedingRequest request)
        {
            var animal = await _animals.GetByIdAsync(request.AnimalId)
                ?? throw new Excep($"Животное {request.AnimalId} не найдено.");

            var schedule = new FeedingSchedule(
                animal.Id,
                request.FeedingTime,
                request.FoodType
            );

            await _schedules.AddAsync(schedule);
        }

        public async Task MarkFeedingCompletedAsync(Guid feedingScheduleId)
        {
            var schedule = await _schedules.GetByIdAsync(feedingScheduleId)
                ?? throw new Excep($"Запись кормления {feedingScheduleId} не найдена.");

            schedule.MarkCompleted();
            await _schedules.UpdateAsync(schedule);
        }
    }
}

