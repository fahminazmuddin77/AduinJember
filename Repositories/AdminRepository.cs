using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class AdminRepository(AppDbContext db) : IAdminRepository
{
    public async Task<Admin?> GetByIdAsync(Guid id) =>
        await db.Admins.FindAsync(id);

    public async Task<Admin?> GetByEmailAsync(string email) =>
        await db.Admins.FirstOrDefaultAsync(a => a.Email == email);

    public async Task<bool> IsAdminAsync(Guid id) =>
        await db.Admins.AnyAsync(a => a.IdAdmin == id);

    public async Task<Admin> CreateAsync(Admin admin)
    {
        db.Admins.Add(admin);
        await db.SaveChangesAsync();
        return admin;
    }

    public async Task<Admin> UpdateAsync(Admin admin)
    {
        db.Admins.Update(admin);
        await db.SaveChangesAsync();
        return admin;
    }
}
