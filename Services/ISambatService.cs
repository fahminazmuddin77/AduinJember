using AduinJember.DTOs;

namespace AduinJember.Services;

public interface ISambatService
{
    Task<SambatDto> CreateAsync(Guid userId, CreateSambatRequest request);
    Task<IEnumerable<SambatDto>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<SambatDto>> GetAllAsync(string? status, string? kategori);
    Task<SambatDto> UpdateStatusAsync(Guid id, string status, Guid adminId, string? catatan);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<RiwayatSambatDto>> GetRiwayatAsync(string? kategori);
    Task<IEnumerable<RiwayatSambatDto>> GetRiwayatBySambatAsync(Guid idSambat);
}