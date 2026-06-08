using AduinJember.Configuration;
using AduinJember.DTOs;
using AduinJember.Models;
using AduinJember.Repositories;
using Supabase.Gotrue;
using User = AduinJember.Models.User;

namespace AduinJember.Services;

public class AuthService(
    Supabase.Client supabase,
    IUserRepository userRepo,
    IAdminRepository adminRepo) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // 1. Daftarkan akun ke Supabase Auth
        var session = await supabase.Auth.SignUp(request.Email, request.Password);

        if (session?.User == null)
            throw new ArgumentException("Registrasi gagal. Email mungkin sudah terdaftar.");

        var uid = Guid.Parse(session.User.Id!);

        // 2. Insert data ke tabel users
        var user = new User
        {
            IdUser = uid,
            Nama   = request.Nama,
            Email  = request.Email,
            Nik    = request.Nik
        };
        await userRepo.CreateAsync(user);

        return new AuthResponse(
            AccessToken : session.AccessToken!,
            TokenType   : "bearer",
            ExpiresIn   : (int)session.ExpiresIn,
            Role        : "user",
            Profile     : MapUserToDto(user)
        );
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // 1. Login ke Supabase Auth
        var session = await supabase.Auth.SignIn(request.Email, request.Password);

        if (session?.User == null)
            throw new UnauthorizedAccessException("Email atau password salah.");

        var uid = Guid.Parse(session.User.Id!);

        // 2. Cek apakah user adalah admin
        var isAdmin = await adminRepo.IsAdminAsync(uid);

        if (isAdmin)
        {
            var admin = await adminRepo.GetByIdAsync(uid)
                ?? throw new KeyNotFoundException("Data admin tidak ditemukan.");

            return new AuthResponse(
                AccessToken : session.AccessToken!,
                TokenType   : "bearer",
                ExpiresIn   : (int)session.ExpiresIn,
                Role        : "admin",
                Profile     : new UserProfileDto(
                    admin.IdAdmin, admin.Nama, admin.Email,
                    null, admin.FotoProfil, admin.CreatedAt)
            );
        }

        // 3. Ambil data user biasa
        var user = await userRepo.GetByIdAsync(uid)
            ?? throw new KeyNotFoundException("Data user tidak ditemukan.");

        return new AuthResponse(
            AccessToken : session.AccessToken!,
            TokenType   : "bearer",
            ExpiresIn   : (int)session.ExpiresIn,
            Role        : "user",
            Profile     : MapUserToDto(user)
        );
    }

    private static UserProfileDto MapUserToDto(User u) =>
        new(u.IdUser, u.Nama, u.Email, u.Nik, u.FotoProfil, u.CreatedAt);
}
