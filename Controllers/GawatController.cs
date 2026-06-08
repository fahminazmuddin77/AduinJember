using System.Security.Claims;
using AduinJember.DTOs;
using AduinJember.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/gawat")]
[Authorize]
public class GawatController(IGawatService gawatService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin =>
        User.IsInRole("admin") || User.IsInRole("service_role");

    /// <summary>User: kirim laporan darurat + GPS otomatis</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GawatDto>>> Create([FromBody] CreateGawatRequest request)
    {
        var result = await gawatService.CreateAsync(CurrentUserId, request);
        return Created($"/api/gawat/{result.IdGawat}",
            new ApiResponse<GawatDto>(true, "Laporan darurat berhasil dikirim.", result));
    }

    /// <summary>User: lihat riwayat darurat sendiri | Admin: pantau semua</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<GawatDto>>>> GetAll([FromQuery] string? status)
    {
        IEnumerable<GawatDto> result = IsAdmin
            ? await gawatService.GetAllAsync(status)
            : await gawatService.GetByUserIdAsync(CurrentUserId);

        return Ok(new ApiResponse<IEnumerable<GawatDto>>(true, "OK", result));
    }

    /// <summary>Admin: update status penanganan darurat</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<GawatDto>>> UpdateStatus(
        Guid id, [FromBody] UpdateGawatStatusRequest request)
    {
        if (!IsAdmin) return Forbid();

        var result = await gawatService.UpdateStatusAsync(id, request.Status, CurrentUserId);
        return Ok(new ApiResponse<GawatDto>(true, "Status penanganan diperbarui.", result));
    }
}
