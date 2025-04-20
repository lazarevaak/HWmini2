using System;
using Microsoft.AspNetCore.Mvc;
using ZooApplication.Interfaces;
using ZooDomain.Entities;
using ZooPresentation.DTOs;

namespace ZooPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosures;

        public EnclosuresController(IEnclosureRepository enclosures) { _enclosures = enclosures; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _enclosures.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var enclosure = await _enclosures.GetByIdAsync(id);
            if (enclosure is null) return NotFound();
            return Ok(enclosure);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnclosureDto dto)
        {
            var enclosure = new Enclosure(dto.Type, dto.Area, dto.Capacity);
            await _enclosures.AddAsync(enclosure);
            return CreatedAtAction(nameof(Get), new { id = enclosure.Id }, enclosure);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var enclosure = await _enclosures.GetByIdAsync(id);
            if (enclosure is null) return NotFound();
            await _enclosures.RemoveAsync(enclosure);
            return NoContent();
        }
    }
}

