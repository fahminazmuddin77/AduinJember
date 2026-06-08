using AduinJember.Models;

namespace AduinJember.Repositories;

public interface IWoroWoroRepository
{
    Task<WoroWoro?> GetByIdAsync(Guid id);
    Task<IEnumerable<WoroWoro>> GetAllAsync(string? kategori = null);
    Task<WoroWoro> CreateAsync(WoroWoro woro);
    Task<WoroWoro> UpdateAsync(Guid id, string? judul, string? konten, string? kategori);
    Task DeleteAsync(Guid id);
}
