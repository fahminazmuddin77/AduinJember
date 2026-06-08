using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class GawatRepository(AppDbContext db) : IGawatRepository
{
    public async Task<Gawat?> GetByIdAsync(Guid id) =>
        await db.Gawats
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.IdGawat == id);

    public async Task<IEnumerable<Gawat>> GetByUserIdAsync(Guid userId) =>
        await db.Gawats
            .Where(g => g.IdUser == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Gawat>> GetAllAsync(string? status = null)
    {
        var query = db.Gawats
            .Include(g => g.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(g => g.Status == status);

        return await query.OrderByDescending(g => g.CreatedAt).ToListAsync();
    }

    public async Task<Gawat> CreateAsync(Gawat gawat)
    {
        db.Gawats.Add(gawat);
        await db.SaveChangesAsync();
        return gawat;
    }

    public async Task<Gawat> UpdateStatusAsync(Guid id, string status, Guid adminId)
    {
        var gawat = await db.Gawats.FindAsync(id)
            ?? throw new KeyNotFoundException("Laporan darurat tidak ditemukan.");

        gawat.Status  = status;
        gawat.IdAdmin = adminId;

        await db.SaveChangesAsync();
        return gawat;
    }
}
