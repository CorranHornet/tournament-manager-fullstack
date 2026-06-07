using ApplicationLayer.Dtos;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;

namespace ApplicationLayer.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ITournamentRepository _tournamentRepository;

        public GameService(
            IGameRepository gameRepository,
            ITournamentRepository tournamentRepository)
        {
            _gameRepository = gameRepository;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<IEnumerable<GameResponseDTO>> GetAllAsync(int? tournamentId = null, string? search = null)
        {
            var games = await _gameRepository.GetAllAsync(tournamentId, search);

            return games.Select(g => new GameResponseDTO
            {
                Id = g.Id,
                Title = g.Title,
                Time = g.Time,
                TournamentId = g.TournamentId
            });
        }

        public async Task<GameResponseDTO?> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);

            if (game == null)
                return null;

            return new GameResponseDTO
            {
                Id = game.Id,
                Title = game.Title,
                Time = game.Time,
                TournamentId = game.TournamentId
            };
        }

        public async Task<GameResponseDTO?> CreateAsync(GameCreateDTO dto)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(dto.TournamentId);

            if (tournament == null)
            {
                return null;
            }

            var game = new Game
            {
                Title = dto.Title,
                Time = dto.Time,
                TournamentId = dto.TournamentId
            }; 

            await _gameRepository.AddAsync(game);
            await _gameRepository.SaveAsync();

            return new GameResponseDTO
            {
                Id = game.Id,
                Title = game.Title,
                Time = game.Time,
                TournamentId = game.TournamentId
            };
        }

        public async Task<bool> UpdateAsync(int id, GameUpdateDTO dto)
        {
            var game = await _gameRepository.GetByIdAsync(id);

            if (game == null)
                return false;

            game.Title = dto.Title;
            game.Time = dto.Time;

            await _gameRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);

            if (game == null)
                return false;

            await _gameRepository.DeleteAsync(game);
            await _gameRepository.SaveAsync();

            return true;
        }
    }
}