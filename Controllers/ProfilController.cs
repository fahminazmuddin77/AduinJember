using System.Security.Claims;
using AduinJember.DTOs;
using AduinJember.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/profil")]
[Authorize]
public class ProfilController(
    IUserRepository userRepo,
    IAdminRepository adminRepo) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin =>
        User.IsInRole("admin") || User.IsInRole("service_role");

    /// <summary>Lihat profil sendiri (user atau admin)</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfil()
    {
        if (IsAdmin)
        {
            var admin = await adminRepo.GetByIdAsync(CurrentUserId)
                ?? throw new KeyNotFoundException("Profil admin tidak ditemukan.");

            return Ok(new ApiResponse<UserProfileDto>(true, "OK", new UserProfileDto(
                admin.IdAdmin, admin.Nama, admin.Email,
                null, admin.FotoProfil, admin.CreatedAt)));
        }

        var user = await userRepo.GetByIdAsync(CurrentUserId)
            ?? throw new KeyNotFoundException("Profil user tidak ditemukan.");

        return Ok(new ApiResponse<UserProfileDto>(true, "OK",
            new UserProfileDto(user.IdUser, user.Nama, user.Email,
                user.Nik, user.FotoProfil, user.CreatedAt)));
    }

    /// <summary>Update profil sendiri</summary>
    [HttpPatch]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateProfil([FromBody] UpdateUserRequest request)
    {
        var user = await userRepo.GetByIdAsync(CurrentUserId)
            ?? throw new KeyNotFoundException("User tidak ditemukan.");

        if (request.Nama        != null) user.Nama        = request.Nama;
        if (request.Nik         != null) user.Nik         = request.Nik;
        if (request.FotoProfil  != null) user.FotoProfil  = request.FotoProfil;

        var updated = await userRepo.UpdateAsync(user);

        return Ok(new ApiResponse<UserProfileDto>(true, "Profil berhasil diperbarui.",
            new UserProfileDto(updated.IdUser, updated.Nama, updated.Email,
                updated.Nik, updated.FotoProfil, updated.CreatedAt)));
    }

    /// <summary>Admin: lihat semua user</summary>
    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserProfileDto>>>> GetAllUsers()
    {
        if (!IsAdmin) return Forbid();

        var users = await userRepo.GetAllAsync();
        var result = users.Select(u => new UserProfileDto(
            u.IdUser, u.Nama, u.Email, u.Nik, u.FotoProfil, u.CreatedAt));

        return Ok(new ApiResponse<IEnumerable<UserProfileDto>>(true, "OK", result));
    }
}
