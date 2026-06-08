using AduinJember.DTOs;

namespace AduinJember.Services;

public interface IWoroWoroService
{
    Task<IEnumerable<WoroWoroDto>> GetAllAsync(string? kategori);
    Task<WoroWoroDto> CreateAsync(Guid adminId, CreateWoroWoroRequest request);
    Task<WoroWoroDto> UpdateAsync(Guid id, UpdateWoroWoroRequest request);
    Task DeleteAsync(Guid id);
}
