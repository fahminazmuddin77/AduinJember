using System.Security.Claims;
using AduinJember.DTOs;
using AduinJember.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/woro-woro")]
public class WoroWoroController(IWoroWoroService woroService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin =>
        User.IsInRole("admin") || User.IsInRole("service_role");

    /// <summary>Semua user: lihat informasi publik</summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<WoroWoroDto>>>> GetAll([FromQuery] string? kategori)
    {
        var result = await woroService.GetAllAsync(kategori);
        return Ok(new ApiResponse<IEnumerable<WoroWoroDto>>(true, "OK", result));
    }

    /// <summary>Admin: tambah informasi baru</summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<WoroWoroDto>>> Create([FromBody] CreateWoroWoroRequest request)
    {
        if (!IsAdmin) return Forbid();

        var result = await woroService.CreateAsync(CurrentUserId, request);
        return Created($"/api/woro-woro/{result.IdWoro}",
            new ApiResponse<WoroWoroDto>(true, "Informasi berhasil ditambahkan.", result));
    }

    /// <summary>Admin: edit informasi</summary>
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<WoroWoroDto>>> Update(
        Guid id, [FromBody] UpdateWoroWoroRequest request)
    {
        if (!IsAdmin) return Forbid();

        var result = await woroService.UpdateAsync(id, request);
        return Ok(new ApiResponse<WoroWoroDto>(true, "Informasi berhasil diperbarui.", result));
    }

    /// <summary>Admin: hapus informasi</summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        if (!IsAdmin) return Forbid();

        await woroService.DeleteAsync(id);
        return Ok(new ApiResponse<object>(true, "Informasi berhasil dihapus.", null));
    }
}
