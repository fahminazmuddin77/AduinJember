using AduinJember.Configuration;
using AduinJember.Models;
using Microsoft.EntityFrameworkCore;

namespace AduinJember.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) =>
        await db.Users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await db.Users.OrderBy(u => u.Nama).ToListAsync();

    public async Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await db.Users.FindAsync(id)
            ?? throw new KeyNotFoundException("User tidak ditemukan.");
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }
}
