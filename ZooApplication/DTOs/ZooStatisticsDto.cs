using System;
namespace ZooApplication.DTOs
{
    public sealed class ZooStatisticsDto
    {
        public int TotalAnimals { get; init; }
        public int TotalEnclosures { get; init; }
        public int FreeEnclosures { get; init; }
        public int UpcomingFeedings { get; init; }
    }
}

