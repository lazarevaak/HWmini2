using System;
using Microsoft.AspNetCore.Mvc;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;
using ZooApplication.Services;
using ZooDomain;
using ZooDomain.Entities;
using ZooDomain.Enum;
using ZooPresentation.DTOs;

namespace ZooPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animals;
        private readonly IAnimalTransferService _transferService;

        public AnimalsController(
            IAnimalRepository animals,
            IAnimalTransferService transferService)
        {
            _animals = animals;
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _animals.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var animal = await _animals.GetByIdAsync(id);
            if (animal is null) return NotFound();
            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnimalDto dto)
        {
            var favoriteFood = new Food(dto.FavoriteFoodName, dto.FavoriteFoodCalories);
            var gender = ParseGender(dto.Gender);
            var animal = new Animal(
                dto.Species,
                dto.Name,
                dto.DateOfBirth,
                gender,
                favoriteFood
            );
            await _animals.AddAsync(animal);
            return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var animal = await _animals.GetByIdAsync(id);
            if (animal is null) return NotFound();
            await _animals.RemoveAsync(animal);
            return NoContent();
        }

        [HttpPost("{id:guid}/transfer")]
        public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferAnimalRequest request)
        {
            if (id != request.AnimalId) return BadRequest();
            await _transferService.TransferAsync(request);
            return NoContent();
        }

        private Gender ParseGender(string gender)
        {
            // TODO: map string to Gender value object
            return new Gender();
        }
    }
}

