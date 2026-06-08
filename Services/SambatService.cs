using AduinJember.DTOs;
using AduinJember.Models;
using AduinJember.Repositories;

namespace AduinJember.Services;

public class SambatService(
    ISambatRepository repo,
    IRiwayatRepository riwayatRepo) : ISambatService
{
    private static readonly string[] ValidStatus =
        ["menunggu", "diproses", "ditindaklanjuti", "selesai"];

    private static readonly string[] ValidKategori =
        ["Sosial", "Infrastruktur", "Layanan Umum"];

    public async Task<SambatDto> CreateAsync(Guid userId, CreateSambatRequest req)
    {
        if (req.Kategori != null && !ValidKategori.Contains(req.Kategori))
            throw new ArgumentException($"Kategori tidak valid. Pilihan: {string.Join(", ", ValidKategori)}");

        var sambat = new Sambat
        {
            IdUser = userId,
            Judul = req.Judul,
            Deskripsi = req.Deskripsi,
            FotoUrl = req.FotoUrl,
            Latitude = req.Latitude,
            Longitude = req.Longitude,
            AlamatLengkap = req.AlamatLengkap,
            Kategori = req.Kategori,
            Status = "menunggu"
        };
        var created = await repo.CreateAsync(sambat);
        return MapToDto(created);
    }

    public async Task<IEnumerable<SambatDto>> GetByUserIdAsync(Guid userId)
    {
        var list = await repo.GetByUserIdAsync(userId);
        return list.Select(MapToDto);
    }

    public async Task<IEnumerable<SambatDto>> GetAllAsync(string? status, string? kategori)
    {
        if (status != null && !ValidStatus.Contains(status))
            throw new ArgumentException($"Status tidak valid. Pilihan: {string.Join(", ", ValidStatus)}");
        if (kategori != null && !ValidKategori.Contains(kategori))
            throw new ArgumentException($"Kategori tidak valid. Pilihan: {string.Join(", ", ValidKategori)}");

        var list = await repo.GetAllAsync(status, kategori);
        return list.Select(MapToDto);
    }

    public async Task<SambatDto> UpdateStatusAsync(Guid id, string status, Guid adminId, string? catatan)
    {
        if (!ValidStatus.Contains(status))
            throw new ArgumentException($"Status tidak valid. Pilihan: {string.Join(", ", ValidStatus)}");

        var existing = await repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Laporan tidak ditemukan.");
        var statusLama = existing.Status;

        var updated = await repo.UpdateStatusAsync(id, status, adminId);

        await riwayatRepo.CreateAsync(new RiwayatSambat
        {
            IdSambat = id,
            IdAdmin = adminId,
            StatusLama = statusLama,
            StatusBaru = status,
            Catatan = catatan
        });

        return MapToDto(updated);
    }

    public Task DeleteAsync(Guid id) => repo.DeleteAsync(id);

    public async Task<IEnumerable<RiwayatSambatDto>> GetRiwayatAsync(string? kategori)
    {
        if (kategori != null && !ValidKategori.Contains(kategori))
            throw new ArgumentException($"Kategori tidak valid. Pilihan: {string.Join(", ", ValidKategori)}");

        var list = await riwayatRepo.GetAllAsync(kategori);
        return list.Select(MapRiwayatToDto);
    }

    public async Task<IEnumerable<RiwayatSambatDto>> GetRiwayatBySambatAsync(Guid idSambat)
    {
        var list = await riwayatRepo.GetBySambatIdAsync(idSambat);
        return list.Select(MapRiwayatToDto);
    }

    private static SambatDto MapToDto(Sambat s) => new(
        s.IdSambat, s.IdUser, s.IdAdmin,
        s.Judul, s.Deskripsi, s.FotoUrl,
        s.Latitude, s.Longitude, s.AlamatLengkap,
        s.Status, s.Kategori, s.CreatedAt, s.UpdatedAt,
        s.User?.Nama, s.User?.Email
    );

    private static RiwayatSambatDto MapRiwayatToDto(RiwayatSambat r) => new(
        r.IdRiwayat, r.IdSambat, r.IdAdmin,
        r.StatusLama, r.StatusBaru, r.Catatan,
        r.CreatedAt, r.Admin?.Nama,
        r.Sambat?.Judul, r.Sambat?.Kategori
    );
}