using ApplicationLayer.Dtos;
using ApplicationLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _service;
        public TournamentsController(ITournamentService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetALl(string? search)
        {
            var tournaments = await _service.GetAllAsync(search);
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var tournament = await _service.GetByIdAsync(id);

            if (tournament == null)
                return NotFound();

            return Ok(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TournamentCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TournamentUpdateDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}