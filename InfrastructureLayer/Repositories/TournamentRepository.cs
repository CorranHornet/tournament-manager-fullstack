using Microsoft.EntityFrameworkCore;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;
using InfrastructureLayer.Database;

namespace InfrastructureLayer.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly AppDbContext _context;

        public TournamentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tournament>> GetAllAsync()
            => await _context.Tournaments
                .Include(t => t.Games)
                .ToListAsync();

        public async Task<Tournament?> GetByIdAsync(int id)
            => await _context.Tournaments
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task AddAsync(Tournament tournament)
            => await _context.Tournaments.AddAsync(tournament);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public async Task DeleteAsync(Tournament tournament)
        {
            _context.Tournaments.Remove(tournament);
            await Task.CompletedTask;
        }
    }
}