using DomainLayer.Models;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync(int? tournamentId, string? search);
    Task<Game?> GetByIdAsync(int id);
    Task AddAsync(Game game);
    Task DeleteAsync(Game game);
    Task SaveAsync();
}