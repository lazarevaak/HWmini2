using System;
using Xunit;
using ZooDomain;
using ZooDomain.Actions;
using ZooDomain.Entities;

namespace ZooTest
{
    public class FeedingScheduleTests
    {
        [Fact]
        public void Ctor_InvalidArgs_ShouldThrow()
        {
            var id = Guid.NewGuid();
            var past = DateTime.UtcNow.AddMinutes(-5);

            Assert.Throws<Excep>(() => new FeedingSchedule(Guid.Empty, DateTime.UtcNow.AddHours(1), "Food"));
            Assert.Throws<Excep>(() => new FeedingSchedule(id, past, "Food"));
            Assert.Throws<Excep>(() => new FeedingSchedule(id, DateTime.UtcNow.AddHours(1), ""));
        }

        [Fact]
        public void Ctor_Valid_Should_SetDefaults()
        {
            var id = Guid.NewGuid();
            var ft = DateTime.UtcNow.AddHours(2);
            var fs = new FeedingSchedule(id, ft, "F");

            Assert.Equal(id, fs.AnimalId);
            Assert.Equal(ft, fs.FeedingTime);
            Assert.Equal("F", fs.FoodType);
            Assert.False(fs.IsCompleted);
        }

        [Fact]
        public void Reschedule_Invalid_ShouldThrow_And_ResetCompleted()
        {
            var id = Guid.NewGuid();
            var fs = new FeedingSchedule(id, DateTime.UtcNow.AddHours(1), "F");

            Assert.Throws<Excep>(() => fs.Reschedule(DateTime.UtcNow.AddHours(-1)));
            fs.MarkCompleted();
            var newTime = DateTime.UtcNow.AddHours(3);
            fs.Reschedule(newTime);

            Assert.Equal(newTime, fs.FeedingTime);
            Assert.False(fs.IsCompleted);
        }

        [Fact]
        public void MarkCompleted_Should_RaiseEvent_And_ThrowOnSecondCall()
        {
            var id = Guid.NewGuid();
            var fs = new FeedingSchedule(id, DateTime.UtcNow.AddMinutes(30), "F");

            fs.MarkCompleted();
            Assert.True(fs.IsCompleted);

            var ev = Assert.Single(fs.DomainEvents.OfType<FeedingTimeEvent>());
            Assert.Equal(fs.AnimalId, ev.AnimalId);

            Assert.Throws<Excep>(() => fs.MarkCompleted());
        }
    }
}

