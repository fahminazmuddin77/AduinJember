using AduinJember.Models;

namespace AduinJember.Repositories;

public interface IGawatRepository
{
    Task<Gawat?> GetByIdAsync(Guid id);
    Task<IEnumerable<Gawat>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Gawat>> GetAllAsync(string? status = null);
    Task<Gawat> CreateAsync(Gawat gawat);
    Task<Gawat> UpdateStatusAsync(Guid id, string status, Guid adminId);
}
