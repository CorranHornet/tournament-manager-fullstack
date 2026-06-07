using ApplicationLayer.Dtos;

namespace ApplicationLayer.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentResponseDTO>> GetAllAsync(string? search = null);
        Task<TournamentResponseDTO?> GetByIdAsync(int id);
        Task<TournamentResponseDTO> CreateAsync(TournamentCreateDTO dto);
        Task<bool> UpdateAsync(int id, TournamentUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}