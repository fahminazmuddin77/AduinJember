using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class RiwayatRepository(AppDbContext db) : IRiwayatRepository
{
    public async Task<IEnumerable<RiwayatSambat>> GetAllAsync(string? kategori = null)
    {
        var query = db.RiwayatSambats
            .Include(r => r.Sambat)
            .Include(r => r.Admin)
            .AsQueryable();

        if (!string.IsNullOrEmpty(kategori))
            query = query.Where(r => r.Sambat!.Kategori == kategori);

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<RiwayatSambat>> GetBySambatIdAsync(Guid idSambat) =>
        await db.RiwayatSambats
            .Include(r => r.Admin)
            .Where(r => r.IdSambat == idSambat)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<RiwayatSambat> CreateAsync(RiwayatSambat riwayat)
    {
        db.RiwayatSambats.Add(riwayat);
        await db.SaveChangesAsync();
        return riwayat;
    }
}