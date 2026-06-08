using AduinJember.DTOs;
using AduinJember.Models;
using AduinJember.Repositories;

namespace AduinJember.Services;

public class GawatService(IGawatRepository repo) : IGawatService
{
    private static readonly string[] ValidJenis  = ["Kecelakaan", "Kriminal", "Bencana"];
    private static readonly string[] ValidStatus = ["Mencari Bantuan", "Ditangani", "Selesai"];

    public async Task<GawatDto> CreateAsync(Guid userId, CreateGawatRequest req)
    {
        if (!ValidJenis.Contains(req.JenisDarurat))
            throw new ArgumentException($"Jenis darurat tidak valid. Pilihan: {string.Join(", ", ValidJenis)}");

        var gawat = new Gawat
        {
            IdUser        = userId,
            JenisDarurat  = req.JenisDarurat,
            Latitude      = req.Latitude,
            Longitude     = req.Longitude,
            Status        = "Mencari Bantuan"
        };

        var created = await repo.CreateAsync(gawat);
        return MapToDto(created);
    }

    public async Task<IEnumerable<GawatDto>> GetByUserIdAsync(Guid userId)
    {
        var list = await repo.GetByUserIdAsync(userId);
        return list.Select(MapToDto);
    }

    public async Task<IEnumerable<GawatDto>> GetAllAsync(string? status)
    {
        if (status != null && !ValidStatus.Contains(status))
            throw new ArgumentException($"Status tidak valid. Pilihan: {string.Join(", ", ValidStatus)}");

        var list = await repo.GetAllAsync(status);
        return list.Select(MapToDto);
    }

    public async Task<GawatDto> UpdateStatusAsync(Guid id, string status, Guid adminId)
    {
        if (!ValidStatus.Contains(status))
            throw new ArgumentException($"Status tidak valid. Pilihan: {string.Join(", ", ValidStatus)}");

        var updated = await repo.UpdateStatusAsync(id, status, adminId);
        return MapToDto(updated);
    }

    private static GawatDto MapToDto(Gawat g) => new(
        g.IdGawat, g.IdUser, g.IdAdmin,
        g.JenisDarurat, g.Latitude, g.Longitude,
        g.Status, g.CreatedAt,
        g.User?.Nama, g.User?.Email
    );
}
