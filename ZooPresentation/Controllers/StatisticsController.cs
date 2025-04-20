using System;
using Microsoft.AspNetCore.Mvc;
using ZooApplication.DTOs;
using ZooApplication.Services;

namespace ZooPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IZooStatisticsService _statsService;

        public StatisticsController(IZooStatisticsService statsService) { _statsService = statsService; }

        [HttpGet]
        public async Task<ZooStatisticsDto> Get() => await _statsService.GetStatisticsAsync();
    }
}

