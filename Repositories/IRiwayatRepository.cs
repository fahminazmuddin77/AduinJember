using AduinJember.Models;

namespace AduinJember.Repositories;

public interface IRiwayatRepository
{
    Task<IEnumerable<RiwayatSambat>> GetAllAsync(string? kategori = null);
    Task<IEnumerable<RiwayatSambat>> GetBySambatIdAsync(Guid idSambat);
    Task<RiwayatSambat> CreateAsync(RiwayatSambat riwayat);
}