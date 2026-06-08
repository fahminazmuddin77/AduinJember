using AduinJember.DTOs;
using AduinJember.Services;
using Microsoft.AspNetCore.Mvc;

namespace AduinJember.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Registrasi akun user baru</summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request);
        return Ok(new ApiResponse<AuthResponse>(true, "Registrasi berhasil.", result));
    }

    /// <summary>Login user atau admin</summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return Ok(new ApiResponse<AuthResponse>(true, "Login berhasil.", result));
    }
}
