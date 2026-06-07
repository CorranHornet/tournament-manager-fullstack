using ApplicationLayer.Dtos;

namespace ApplicationLayer.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameResponseDTO>> GetAllAsync(int? tournamentId = null, string? search = null);
        Task<GameResponseDTO?> GetByIdAsync(int id);
        Task<GameResponseDTO> CreateAsync(GameCreateDTO dto);
        Task<bool> UpdateAsync(int id, GameUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}