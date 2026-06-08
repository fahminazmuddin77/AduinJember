using System.Security.Claims;
using AduinJember.DTOs;
using AduinJember.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/sambat")]
[Authorize]
public class SambatController(ISambatService sambatService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin =>
        User.IsInRole("service_role") || User.IsInRole("admin");

    /// <summary>User: kirim laporan baru</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SambatDto>>> Create(
        [FromBody] CreateSambatRequest request)
    {
        var result = await sambatService.CreateAsync(CurrentUserId, request);
        return Created($"/api/sambat/{result.IdSambat}",
            new ApiResponse<SambatDto>(true, "Laporan berhasil dikirim.", result));
    }

    /// <summary>User: laporan sendiri | Admin: semua laporan, filter by status & kategori</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SambatDto>>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? kategori)
    {
        IEnumerable<SambatDto> result = IsAdmin
            ? await sambatService.GetAllAsync(status, kategori)
            : await sambatService.GetByUserIdAsync(CurrentUserId);

        return Ok(new ApiResponse<IEnumerable<SambatDto>>(true, "OK", result));
    }

    /// <summary>Admin: update status + otomatis catat ke riwayat</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<SambatDto>>> UpdateStatus(
        Guid id, [FromBody] UpdateSambatStatusRequest request)
    {
        if (!IsAdmin) return Forbid();
        var result = await sambatService.UpdateStatusAsync(
            id, request.Status, CurrentUserId, request.Catatan);
        return Ok(new ApiResponse<SambatDto>(true, "Status laporan diperbarui.", result));
    }

    /// <summary>Admin: hapus laporan</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        if (!IsAdmin) return Forbid();
        await sambatService.DeleteAsync(id);
        return Ok(new ApiResponse<object>(true, "Laporan berhasil dihapus.", null));
    }

    /// <summary>Admin: lihat semua riwayat, filter by kategori</summary>
    [HttpGet("riwayat")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RiwayatSambatDto>>>> GetRiwayat(
        [FromQuery] string? kategori)
    {
        if (!IsAdmin) return Forbid();
        var result = await sambatService.GetRiwayatAsync(kategori);
        return Ok(new ApiResponse<IEnumerable<RiwayatSambatDto>>(true, "OK", result));
    }

    /// <summary>Lihat riwayat by id laporan tertentu</summary>
    [HttpGet("{id:guid}/riwayat")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RiwayatSambatDto>>>> GetRiwayatBySambat(
        Guid id)
    {
        var result = await sambatService.GetRiwayatBySambatAsync(id);
        return Ok(new ApiResponse<IEnumerable<RiwayatSambatDto>>(true, "OK", result));
    }
}