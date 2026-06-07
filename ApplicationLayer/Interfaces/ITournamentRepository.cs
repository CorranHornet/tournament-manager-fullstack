using DomainLayer.Models;


namespace ApplicationLayer.Interfaces
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> GetAllAsync();
        Task<Tournament?> GetByIdAsync(int id);

        Task AddAsync(Tournament tournament);
        Task DeleteAsync(Tournament tournament);

        Task SaveAsync();
    }
}