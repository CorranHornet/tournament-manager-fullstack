//using ApplicationLayer.Interfaces;
//using DomainLayer.Models;

//public class FakeGameRepository : IGameRepository
//{
//    private readonly List<Game> _games = new();

//    public Task<List<Game>> GetAllAsync()
//        => Task.FromResult(_games);

//    public Task<Game?> GetByIdAsync(int id)
//        => Task.FromResult(_games.FirstOrDefault(x => x.Id == id));

//    public Task AddAsync(Game game)
//    {
//        game.Id = _games.Count + 1;
//        _games.Add(game);
//        return Task.CompletedTask;
//    }

//    public Task DeleteAsync(Game game)
//    {
//        _games.Remove(game);
//        return Task.CompletedTask;
//    }

//    public Task SaveAsync()
//        => Task.CompletedTask;

//    public Task<bool> TournamentExists(int tournamentId)
//        => Task.FromResult(true);
//}


//using ApplicationLayer.Interfaces;
//using DomainLayer.Models;

//public class FakeGameRepository : IGameRepository
//{
//    public Task<List<Game>> GetAllAsync()
//    {
//        return Task.FromResult(new List<Game>
//        {
//            new Game { Id = 1, Title = "Fake Game", Time = DateTime.Now }
//        });
//    }

//    public Task<Game?> GetByIdAsync(int id)
//    {
//        return Task.FromResult<Game?>(new Game { Id = id, Title = "Fake" });
//    }

//    public Task AddAsync(Game game) => Task.CompletedTask;
//    public Task DeleteAsync(Game game) => Task.CompletedTask;
//    public Task SaveAsync() => Task.CompletedTask;

//    public Task<bool> TournamentExists(int tournamentId)
//    {
//        return Task.FromResult(true);
//    }
//}