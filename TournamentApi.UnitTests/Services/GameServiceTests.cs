using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Services;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;
using Xunit;

namespace TournamentApi.UnitTests.Services
{
    public class GameServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenTournamentDoesNotExist()
        {
            // 1. Arrange
            var fakeGameRepo = new FakeGameRepository();
            var fakeTournamentRepo = new FakeTournamentRepository();
            var gameService = new GameService(fakeGameRepo, fakeTournamentRepo);

            var createDto = new GameCreateDTO
            {
                Title = "Champions League Final",
                Time = DateTime.Now,
                TournamentId = 99 // Finns inte!
            };

            // 2. Act
            var result = await gameService.CreateAsync(createDto);

            // 3. Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnGame_WhenTournamentExists()
        {
            // 1. Arrange
            var fakeGameRepo = new FakeGameRepository();
            var fakeTournamentRepo = new FakeTournamentRepository();
            var gameService = new GameService(fakeGameRepo, fakeTournamentRepo);

            var existingTournament = new Tournament { Id = 1, Title = "Summer Cup", Description = "Warmup" };
            await fakeTournamentRepo.AddAsync(existingTournament);

            var createDto = new GameCreateDTO
            {
                Title = "Quarter Final",
                Time = DateTime.Now,
                TournamentId = 1
            };

            // 2. Act
            var result = await gameService.CreateAsync(createDto);

            // 3. Assert
            Assert.NotNull(result);
            Assert.Equal("Quarter Final", result.Title);
            Assert.Equal(1, result.TournamentId);
            Assert.Single(fakeGameRepo.GetInternalList());
        }
    }

    // --- HÄR BAKAR VI IN DINA FAKES SÅ ATT ALLT FINNS PÅ SAMMA STÄLLE ---

    public class FakeTournamentRepository : ITournamentRepository
    {
        private readonly List<Tournament> _tournaments = new List<Tournament>();

        public async Task<List<Tournament>> GetAllAsync() => await Task.FromResult(_tournaments.ToList());
        public async Task<Tournament?> GetByIdAsync(int id) => await Task.FromResult(_tournaments.FirstOrDefault(t => t.Id == id));
        public async Task AddAsync(Tournament tournament) { _tournaments.Add(tournament); await Task.CompletedTask; }
        public async Task SaveAsync() => await Task.CompletedTask;
        public async Task DeleteAsync(Tournament tournament) { _tournaments.Remove(tournament); await Task.CompletedTask; }
    }

    public class FakeGameRepository : IGameRepository
    {
        private readonly List<Game> _games = new List<Game>();

        public async Task<List<Game>> GetAllAsync(int? tournamentId, string? search)
        {
            var query = _games.AsQueryable();
            if (tournamentId.HasValue) query = query.Where(g => g.TournamentId == tournamentId.Value);
            return await Task.FromResult(query.ToList());
        }

        public async Task<Game?> GetByIdAsync(int id) => await Task.FromResult(_games.FirstOrDefault(g => g.Id == id));
        public async Task AddAsync(Game game)
        {
            if (game.Id == 0) game.Id = _games.Count > 0 ? _games.Max(g => g.Id) + 1 : 1;
            _games.Add(game);
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(Game game) { _games.Remove(game); await Task.CompletedTask; }
        public async Task SaveAsync() => await Task.CompletedTask;
        public List<Game> GetInternalList() => _games;
    }
}