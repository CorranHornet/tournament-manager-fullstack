using ApplicationLayer.Dtos;
using DomainLayer.Models;

//using TournamentApi.Data;              // ❌ TA BORT (DbContext får inte vara här i Clean Arch service)
using ApplicationLayer.Interfaces;



namespace TournamentApi.Services
{
    public class TournamentService : ITournamentService
    {
        // ❌ NUVARANDE (BROTT MOT CLEAN ARCH)
        // private readonly AppDbContext _context;

        // ✔ NY VERSION (RÄTT)
        private readonly ITournamentRepository _repo;

        // ❌ GAMMAL CONSTRUCTOR
        // public TournamentService(AppDbContext context)
        // {
        //     _context = context;
        // }

        // ✔ NY CONSTRUCTOR (RÄTT CLEAN ARCH)
        public TournamentService(ITournamentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TournamentResponseDTO>> GetAllAsync(string? search = null)
        {
            // ❌ TIDIGARE: _context.Tournaments.AsQueryable()

            var tournaments = await _repo.GetAllAsync();

            // ✔ search ska ligga i service (OK), inte EF
            if (!string.IsNullOrWhiteSpace(search))
            {
                var cleanSearch = search.Trim().ToLower();

                tournaments = tournaments
                    .Where(t => t.Title.ToLower().Contains(cleanSearch))
                    .ToList();
            }

            return tournaments.Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                MaxPlayers = t.MaxPlayers,
            });
        }

        public async Task<TournamentResponseDTO?> GetByIdAsync(int id)
        {
            // ❌ TIDIGARE: _context.Tournaments.Include(...)

            var t = await _repo.GetByIdAsync(id);

            if (t == null) return null;

            return new TournamentResponseDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                MaxPlayers = t.MaxPlayers,

                // ✔ OK att mappa Games här eftersom det redan laddas i repository
                Games = t.Games.Select(g => new GameResponseDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Time = g.Time
                }).ToList()
            };
        }

        public async Task<TournamentResponseDTO> CreateAsync(TournamentCreateDTO dto)
        {
            var tournament = new Tournament
            {
                Title = dto.Title, 
                Description = dto.Description,
                MaxPlayers = dto.MaxPlayers,
                Date = dto.Date
            };

            // ❌ TIDIGARE:
            // _context.Tournaments.Add(tournament);
            // await _context.SaveChangesAsync();

            // ✔ NU:
            await _repo.AddAsync(tournament);
            await _repo.SaveAsync();

            return new TournamentResponseDTO
            {
                Id = tournament.Id,
                Title = tournament.Title,
                Description = tournament.Description,
                MaxPlayers = tournament.MaxPlayers,
            };
        }

        public async Task<bool> UpdateAsync(int id, TournamentUpdateDTO dto)
        {
            // ❌ TIDIGARE: FindAsync via DbContext

            var tournament = await _repo.GetByIdAsync(id);

            if (tournament == null)
                return false;

            tournament.Title = dto.Title;
            tournament.Description = dto.Description;
            tournament.MaxPlayers = dto.MaxPlayers;
            tournament.Date = dto.Date;

            await _repo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tournament = await _repo.GetByIdAsync(id);

            if (tournament == null)
                return false;

            await _repo.DeleteAsync(tournament);
            await _repo.SaveAsync();

            return true;
        }
    }
}