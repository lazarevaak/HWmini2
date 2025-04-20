using System;
using ZooApplication.DTOs;

namespace ZooApplication.Services
{
    public interface IZooStatisticsService
    {
        Task<ZooStatisticsDto> GetStatisticsAsync();
    }
}

