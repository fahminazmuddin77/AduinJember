using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class WoroWoroRepository(AppDbContext db) : IWoroWoroRepository
{
    public async Task<WoroWoro?> GetByIdAsync(Guid id) =>
        await db.WoroWoros
            .Include(w => w.Admin)
            .FirstOrDefaultAsync(w => w.IdWoro == id);

    public async Task<IEnumerable<WoroWoro>> GetAllAsync(string? kategori = null)
    {
        var query = db.WoroWoros.Include(w => w.Admin).AsQueryable();

        if (!string.IsNullOrEmpty(kategori))
            query = query.Where(w => w.Kategori == kategori);

        return await query.OrderByDescending(w => w.CreatedAt).ToListAsync();
    }

    public async Task<WoroWoro> CreateAsync(WoroWoro woro)
    {
        db.WoroWoros.Add(woro);
        await db.SaveChangesAsync();
        return woro;
    }

    public async Task<WoroWoro> UpdateAsync(Guid id, string? judul, string? konten, string? kategori, string? fotoUrl)
    {
        var woro = await db.WoroWoros.FindAsync(id)
            ?? throw new KeyNotFoundException("Woro-woro tidak ditemukan.");

        if (judul    != null) woro.Judul    = judul;
        if (konten   != null) woro.Konten   = konten;
        if (kategori != null) woro.Kategori = kategori;
        if (fotoUrl  != null) woro.FotoUrl  = fotoUrl;

        await db.SaveChangesAsync();
        return woro;
    }

    public async Task DeleteAsync(Guid id)
    {
        var woro = await db.WoroWoros.FindAsync(id)
            ?? throw new KeyNotFoundException("Woro-woro tidak ditemukan.");
        db.WoroWoros.Remove(woro);
        await db.SaveChangesAsync();
    }
}
