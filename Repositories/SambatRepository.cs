using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class SambatRepository(AppDbContext db) : ISambatRepository
{
    public async Task<Sambat?> GetByIdAsync(Guid id) =>
        await db.Sambats
            .Include(s => s.User)
            .Include(s => s.Admin)
            .FirstOrDefaultAsync(s => s.IdSambat == id);

    public async Task<IEnumerable<Sambat>> GetByUserIdAsync(Guid userId) =>
        await db.Sambats
            .Where(s => s.IdUser == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Sambat>> GetAllAsync(string? status = null, string? kategori = null)
    {
        var query = db.Sambats.Include(s => s.User).AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(s => s.Status == status);

        if (!string.IsNullOrEmpty(kategori))
            query = query.Where(s => s.Kategori == kategori);

        return await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    public async Task<Sambat> CreateAsync(Sambat sambat)
    {
        db.Sambats.Add(sambat);
        await db.SaveChangesAsync();
        return sambat;
    }

    public async Task<Sambat> UpdateStatusAsync(Guid id, string status, Guid adminId)
    {
        var sambat = await db.Sambats.FindAsync(id)
            ?? throw new KeyNotFoundException("Laporan tidak ditemukan.");
        sambat.Status = status;
        sambat.IdAdmin = adminId;
        sambat.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return sambat;
    }

    public async Task DeleteAsync(Guid id)
    {
        var sambat = await db.Sambats.FindAsync(id)
            ?? throw new KeyNotFoundException("Laporan tidak ditemukan.");
        db.Sambats.Remove(sambat);
        await db.SaveChangesAsync();
    }
}