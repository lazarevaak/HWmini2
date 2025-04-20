using System;
using Microsoft.AspNetCore.Mvc;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;
using ZooApplication.Services;

namespace ZooPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingSchedulesController : ControllerBase
    {
        private readonly IFeedingOrganizationService _feedingService;
        private readonly IFeedingScheduleRepository _schedules;

        public FeedingSchedulesController(IFeedingOrganizationService feedingService, IFeedingScheduleRepository schedules)
        {
            _feedingService = feedingService;
            _schedules = schedules;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _schedules.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] FeedingRequest request)
        {
            await _feedingService.ScheduleFeedingAsync(request);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _feedingService.MarkFeedingCompletedAsync(id);
            return NoContent();
        }
    }
}

