using AduinJember.Models;

namespace AduinJember.Repositories;

public interface IAdminRepository
{
    Task<Admin?> GetByIdAsync(Guid id);
    Task<Admin?> GetByEmailAsync(string email);
    Task<bool> IsAdminAsync(Guid id);
    Task<Admin> CreateAsync(Admin admin);
    Task<Admin> UpdateAsync(Admin admin);
}
