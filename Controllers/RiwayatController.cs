using AduinJember.DTOs;
using AduinJember.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/riwayat")]
[Authorize]
public class RiwayatController(ISambatService sambatService) : ControllerBase
{
    private bool IsAdmin =>
        User.IsInRole("service_role") || User.IsInRole("admin");

    /// <summary>Admin: lihat semua riwayat perubahan status laporan, filter by kategori</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RiwayatSambatDto>>>> GetAll(
        [FromQuery] string? kategori)
    {
        if (!IsAdmin) return Forbid();
        var result = await sambatService.GetRiwayatAsync(kategori);
        return Ok(new ApiResponse<IEnumerable<RiwayatSambatDto>>(true, "OK", result));
    }

    /// <summary>Lihat riwayat perubahan status berdasarkan ID sambat (laporan) tertentu</summary>
    [HttpGet("sambat/{sambatId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RiwayatSambatDto>>>> GetBySambat(
        Guid sambatId)
    {
        var result = await sambatService.GetRiwayatBySambatAsync(sambatId);
        return Ok(new ApiResponse<IEnumerable<RiwayatSambatDto>>(true, "OK", result));
    }
}
