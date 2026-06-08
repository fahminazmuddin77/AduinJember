using AduinJember.DTOs;

namespace AduinJember.Services;

public interface IGawatService
{
    Task<GawatDto> CreateAsync(Guid userId, CreateGawatRequest request);
    Task<IEnumerable<GawatDto>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<GawatDto>> GetAllAsync(string? status);
    Task<GawatDto> UpdateStatusAsync(Guid id, string status, Guid adminId);
}
