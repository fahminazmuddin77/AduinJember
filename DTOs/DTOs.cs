namespace AduinJember.DTOs;

// ── AUTH ──────────────────────────────────────────────
public record RegisterRequest(string Email, string Password, string Nama, string? Nik);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string AccessToken, string TokenType, int ExpiresIn, string Role, UserProfileDto Profile);

// ── USER ──────────────────────────────────────────────
public record UserProfileDto(Guid IdUser, string Nama, string Email, string? Nik, string? FotoProfil, DateTime CreatedAt);
public record UpdateUserRequest(string? Nama, string? Nik, string? FotoProfil);

// ── ADMIN ─────────────────────────────────────────────
public record AdminProfileDto(Guid IdAdmin, string Nama, string Email, string? Instansi, string? Jabatan, string? FotoProfil);

// ── SAMBAT ────────────────────────────────────────────
public record CreateSambatRequest(
    string Judul,
    string Deskripsi,
    string? FotoUrl,
    double? Latitude,
    double? Longitude,
    string? AlamatLengkap,
    string? Kategori        // Sosial | Infrastruktur | Layanan Umum
);

public record UpdateSambatStatusRequest(
    string Status,          // menunggu | diproses | ditindaklanjuti | selesai
    string? Catatan         // catatan opsional dari admin
);

public record SambatDto(
    Guid IdSambat,
    Guid? IdUser,
    Guid? IdAdmin,
    string Judul,
    string Deskripsi,
    string? FotoUrl,
    double? Latitude,
    double? Longitude,
    string? AlamatLengkap,
    string Status,
    string? Kategori,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? NamaUser,
    string? EmailUser
);

// ── RIWAYAT SAMBAT ────────────────────────────────────
public record RiwayatSambatDto(
    Guid IdRiwayat,
    Guid IdSambat,
    Guid? IdAdmin,
    string StatusLama,
    string StatusBaru,
    string? Catatan,
    DateTime CreatedAt,
    string? NamaAdmin,
    string? JudulSambat,
    string? KategoriSambat
);

// ── GAWAT ─────────────────────────────────────────────
public record CreateGawatRequest(
    string JenisDarurat,    // Kecelakaan | Kriminal | Bencana
    double Latitude,
    double Longitude
);

public record UpdateGawatStatusRequest(
    string Status           // Mencari Bantuan | Ditangani | Selesai
);

public record GawatDto(
    Guid IdGawat,
    Guid? IdUser,
    Guid? IdAdmin,
    string JenisDarurat,
    double Latitude,
    double Longitude,
    string Status,
    DateTime CreatedAt,
    string? NamaUser,
    string? EmailUser
);

// ── WORO-WORO ─────────────────────────────────────────
public record CreateWoroWoroRequest(
    string Judul,
    string Konten,
    string? Kategori,        // Infrastruktur | Kebersihan | Keamanan | Kesehatan | Lainnya
    string? FotoUrl
);

public record UpdateWoroWoroRequest(
    string? Judul,
    string? Konten,
    string? Kategori,
    string? FotoUrl
);

public record WoroWoroDto(
    Guid IdWoro,
    Guid? IdAdmin,
    string Judul,
    string Konten,
    string? Kategori,
    string? FotoUrl,
    DateTime CreatedAt,
    string? NamaAdmin
);

// ── GENERIC ───────────────────────────────────────────
public record ApiResponse<T>(bool Success, string Message, T? Data);
public record PaginatedResponse<T>(bool Success, string Message, IEnumerable<T> Data, int Total, int Page, int PageSize);