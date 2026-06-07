using ApplicationLayer.Interfaces;
using DomainLayer.Models;
using InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;


namespace TournamentApi.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAllAsync(int? tournamentId, string? search)
        {
            var query = _context.Games.AsQueryable();

            if (tournamentId.HasValue)
                query = query.Where(g => g.TournamentId == tournamentId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => EF.Functions.Like(g.Title, $"%{search}%"));

            return await query.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
      
    }
}
