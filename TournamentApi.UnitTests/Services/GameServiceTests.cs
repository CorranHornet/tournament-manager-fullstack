using ApplicationLayer.Services;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;
using NSubstitute;
using Xunit;

namespace TournamentApi.UnitTests.Services
{
    public class GameServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenTournamentDoesNotExist()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            // Simulate that the tournament is not found (returns null)
            mockTournamentRepo.GetByIdAsync(99).Returns(Task.FromResult<Tournament?>(null));

            var createDto = new GameCreateDTO
            {
                Title = "Champions League Final",
                Time = DateTime.Now,
                TournamentId = 99
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
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            var existingTournament = new Tournament { Id = 1, Title = "Summer Cup", Description = "Warmup" };

            // Simulate that the tournament exists when queried by ID 1
            mockTournamentRepo.GetByIdAsync(1).Returns(Task.FromResult<Tournament?>(existingTournament));

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

            // Verify that the repository's AddAsync method was actually called
            await mockGameRepo.Received(1).AddAsync(Arg.Any<Game>());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            // Simulate that the game does not exist in the database (returns null)
            mockGameRepo.GetByIdAsync(999).Returns(Task.FromResult<Game?>(null));

            // 2. Act
            var result = await gameService.GetByIdAsync(999);

            // 3. Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnGameResponseDto_WhenGameExists()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            var existingGame = new Game
            {
                Id = 5,
                Title = "Grand Final",
                Time = DateTime.Now,
                TournamentId = 1
            };

            // Simulate that the game is successfully found in the database
            mockGameRepo.GetByIdAsync(5).Returns(Task.FromResult<Game?>(existingGame));

            // 2. Act
            var result = await gameService.GetByIdAsync(5);

            // 3. Assert
            Assert.NotNull(result);
            Assert.Equal("Grand Final", result.Title);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenGameDoesNotExist()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            // Simulate that the game to delete is not found
            mockGameRepo.GetByIdAsync(999).Returns(Task.FromResult<Game?>(null));

            // 2. Act
            var result = await gameService.DeleteAsync(999);

            // 3. Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenGameIsSuccessfullyDeleted()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            var existingGame = new Game { Id = 10, Title = "Match to delete" };

            // Simulate that the game exists
            mockGameRepo.GetByIdAsync(10).Returns(Task.FromResult<Game?>(existingGame));

            // 2. Act
            var result = await gameService.DeleteAsync(10);

            // 3. Assert
            Assert.True(result);

            // Verify that the repository's DeleteAsync method was invoked with the actual Game object
            await mockGameRepo.Received(1).DeleteAsync(existingGame);
        }
    }
}