using ApplicationLayer.Dtos;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _service;

        public GamesController(IGameService service)
        {
            _service = service;
        }

        // GET /api/games?tournamentId=1&search=abc
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? tournamentId = null, [FromQuery] string? search = null)
        {
            var games = await _service.GetAllAsync(tournamentId, search);

            if (!games.Any())
                return NotFound(new { message = "No games found matching the criteria." });

            return Ok(games);
        }

        // GET /api/games/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _service.GetByIdAsync(id);

            if (game == null)
                return NotFound(new { message = $"Game with ID {id} not found" });

            return Ok(game);
        }

        // POST /api/games
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            if (created == null)
            {
                return NotFound(new
                {
                    message = $"Tournament with ID {dto.TournamentId} does not exist"
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/games/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(id, dto);

            if (!success)
                return NotFound(new { message = $"Game with ID {id} not found" });

            return NoContent();
        }

        // DELETE /api/games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound(new { message = $"Game with ID {id} not found" });

            return NoContent();
        }
    }
}