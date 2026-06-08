using AduinJember.Models;

namespace AduinJember.Repositories;

public interface ISambatRepository
{
    Task<Sambat?> GetByIdAsync(Guid id);
    Task<IEnumerable<Sambat>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Sambat>> GetAllAsync(string? status = null, string? kategori = null);
    Task<Sambat> CreateAsync(Sambat sambat);
    Task<Sambat> UpdateStatusAsync(Guid id, string status, Guid adminId);
    Task DeleteAsync(Guid id);
}