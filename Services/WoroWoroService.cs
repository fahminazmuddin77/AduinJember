using AduinJember.DTOs;
using AduinJember.Models;
using AduinJember.Repositories;

namespace AduinJember.Services;

public class WoroWoroService(IWoroWoroRepository repo) : IWoroWoroService
{
    private static readonly string[] ValidKategori =
        ["Infrastruktur", "Kebersihan", "Keamanan", "Kesehatan", "Lainnya"];

    public async Task<IEnumerable<WoroWoroDto>> GetAllAsync(string? kategori)
    {
        if (kategori != null && !ValidKategori.Contains(kategori))
            throw new ArgumentException($"Kategori tidak valid. Pilihan: {string.Join(", ", ValidKategori)}");

        var list = await repo.GetAllAsync(kategori);
        return list.Select(MapToDto);
    }

    public async Task<WoroWoroDto> CreateAsync(Guid adminId, CreateWoroWoroRequest req)
    {
        if (req.Kategori != null && !ValidKategori.Contains(req.Kategori))
            throw new ArgumentException($"Kategori tidak valid.");

        var woro = new WoroWoro
        {
            IdAdmin  = adminId,
            Judul    = req.Judul,
            Konten   = req.Konten,
            Kategori = req.Kategori,
            FotoUrl  = req.FotoUrl
        };

        var created = await repo.CreateAsync(woro);
        return MapToDto(created);
    }

    public async Task<WoroWoroDto> UpdateAsync(Guid id, UpdateWoroWoroRequest req)
    {
        if (req.Kategori != null && !ValidKategori.Contains(req.Kategori))
            throw new ArgumentException("Kategori tidak valid.");

        var updated = await repo.UpdateAsync(id, req.Judul, req.Konten, req.Kategori, req.FotoUrl);
        return MapToDto(updated);
    }

    public Task DeleteAsync(Guid id) => repo.DeleteAsync(id);

    private static WoroWoroDto MapToDto(WoroWoro w) => new(
        w.IdWoro, w.IdAdmin,
        w.Judul, w.Konten, w.Kategori, w.FotoUrl,
        w.CreatedAt, w.Admin?.Nama
    );
}
