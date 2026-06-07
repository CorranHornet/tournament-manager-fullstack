using System;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Services;
using ApplicationLayer.Interfaces;
using DomainLayer.Models;
using NSubstitute; // <-- Denna gör magin möjlig!
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

            // Vi simulerar att turneringen med ID 99 INTE finns (returnerar null)
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

            // Vi säger till NSubstitute att returnera vår turnering när servicen frågar efter ID 1
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

            // NSubstitute-verifiering: Kollade servicen verkligen att spelet sparades i repot?
            await mockGameRepo.Received(1).AddAsync(Arg.Any<Game>());


        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // 1. Arrange
            var mockGameRepo = Substitute.For<IGameRepository>();
            var mockTournamentRepo = Substitute.For<ITournamentRepository>();
            var gameService = new GameService(mockGameRepo, mockTournamentRepo);

            // Vi simulerar att spelet med ID 999 inte finns i databasen (returnerar null)
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

            // Vi simulerar att spelet med ID 5 faktiskt hittas i databasen
            mockGameRepo.GetByIdAsync(5).Returns(Task.FromResult<Game?>(existingGame));

            // 2. Act
            var result = await gameService.GetByIdAsync(5);

            // 3. Assert
            Assert.NotNull(result);
            Assert.Equal("Grand Final", result.Title);
            Assert.Equal(5, result.Id);
        }
    }
}