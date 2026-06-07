using ApplicationLayer.Dtos;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;

namespace ApplicationLayer.Services
{
    public class GameService : IGameService
    {
        // Define read-only repository interfaces to ensure loose coupling (Clean Architecture)
        private readonly IGameRepository _gameRepository;
        private readonly ITournamentRepository _tournamentRepository;

        // Both repositories are injected via Constructor Injection.
        // By injecting ITournamentRepository, we follow Separation of Concerns:
        // the GameService validates tournament existence without leaking database logic into the Game repository.
        public GameService(
            IGameRepository gameRepository,
            ITournamentRepository tournamentRepository)
        {
            _gameRepository = gameRepository;
            _tournamentRepository = tournamentRepository;
        }

        /// <summary>
        /// Retrieves a list of games, optionally filtered by tournament ID or a search string.
        /// </summary>
        public async Task<IEnumerable<GameResponseDTO>> GetAllAsync(int? tournamentId = null, string? search = null)
        {
            // Fetch raw domain entities from the infrastructure layer via the interface
            var games = await _gameRepository.GetAllAsync(tournamentId, search);

            // Project the Domain objects into safe Data Transfer Objects (DTOs) to abstract the data layer
            return games.Select(g => new GameResponseDTO
            {
                Id = g.Id,
                Title = g.Title,
                Time = g.Time,
                TournamentId = g.TournamentId
            });
        }

        /// <summary>
        /// Retrieves a single game by its unique identifier.
        /// </summary>
        public async Task<GameResponseDTO?> GetByIdAsync(int id)
        {
            // Fetch the domain entity by ID
            var game = await _gameRepository.GetByIdAsync(id);

            // Return null to the Controller if the game does not exist
            if (game == null)
                return null;

            // Map the domain entity to a Response DTO
            return new GameResponseDTO
            {
                Id = game.Id,
                Title = game.Title,
                Time = game.Time,
                TournamentId = game.TournamentId
            };
        }

        /// <summary>
        /// Validates requirements and creates a new game tied to an existing tournament.
        /// </summary>
        public async Task<GameResponseDTO?> CreateAsync(GameCreateDTO dto)
        {
            // CLEAN ARCHITECTURE REFACTOR:
            // Verify that the requested tournament actually exists by checking the Tournament Repository.
            // This decouples the GameRepository from having to know about Tournament database tables.
            var tournament = await _tournamentRepository.GetByIdAsync(dto.TournamentId);

            // If the tournament is missing, abort operation and return null (preventing foreign key violations)
            if (tournament == null)
            {
                return null;
            }

            // Map incoming DTO properties into a fresh Domain Entity
            var game = new Game
            {
                Title = dto.Title,
                Time = dto.Time,
                TournamentId = dto.TournamentId
            };

            // Persist changes to the data store using Unit of Work patterns (Add then Save)
            await _gameRepository.AddAsync(game);
            await _gameRepository.SaveAsync();

            // Return the saved game metadata mapped to a Response DTO
            return new GameResponseDTO
            {
                Id = game.Id,
                Title = game.Title,
                Time = game.Time,
                TournamentId = game.TournamentId
            };
        }

        /// <summary>
        /// Updates the mutable properties of an existing game.
        /// </summary>
        public async Task<bool> UpdateAsync(int id, GameUpdateDTO dto)
        {
            // Retrieve the entity tracking instance from the repository
            var game = await _gameRepository.GetByIdAsync(id);

            // Return false if there is no game matching the ID to update
            if (game == null)
                return false;

            // Apply modifications to the domain entity
            game.Title = dto.Title;
            game.Time = dto.Time;

            // Commit tracking changes to the database
            await _gameRepository.SaveAsync();

            return true;
        }

        /// <summary>
        /// Deletes a game completely from the application database.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            // Retrieve the target entity from the database
            var game = await _gameRepository.GetByIdAsync(id);

            // If it doesn't exist, exit out early and return false
            if (game == null)
                return false;

            // Execute the deletion and save state changes
            await _gameRepository.DeleteAsync(game);
            await _gameRepository.SaveAsync();

            return true;
        }
    }
}