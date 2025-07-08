using AutoMapper;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DrinksController : ControllerBase
    {
        private readonly IDrinkService _service;
        private readonly ILogger<DrinksController> _logger;
        public DrinksController(IDrinkService service, ILogger<DrinksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all drinks.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all drinks.")]
        [ProducesResponseType(typeof(IEnumerable<DrinkDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var drinks = await _service.GetAllAsync();
            return Ok(drinks);
        }

        /// <summary>
        /// Get a drink by ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a drink by ID.")]
        [ProducesResponseType(typeof(DrinkDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var drink = await _service.GetByIdAsync(id);
            if (drink == null) return NotFound();
            return Ok(drink);
        }

        /// <summary>
        /// Add a new drink.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new drink.")]
        [ProducesResponseType(typeof(DrinkDto), 201)]
        public async Task<IActionResult> Add([FromBody] DrinkDto dto)
        {
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing drink.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing drink.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] DrinkDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var updated = await _service.UpdateAsync(dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Delete a drink.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a drink.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
