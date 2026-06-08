namespace AduinJember.Models;

public class User
{
    public Guid IdUser { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Nik { get; set; }
    public string? FotoProfil { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Sambat> Sambats { get; set; } = [];
    public ICollection<Gawat> Gawats { get; set; } = [];
}

public class Admin
{
    public Guid IdAdmin { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Instansi { get; set; }
    public string? Jabatan { get; set; }
    public string? FotoProfil { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Sambat> Sambats { get; set; } = [];
    public ICollection<Gawat> Gawats { get; set; } = [];
    public ICollection<WoroWoro> WoroWoros { get; set; } = [];
}

public class Sambat
{
    public Guid IdSambat { get; set; }
    public Guid? IdUser { get; set; }
    public Guid? IdAdmin { get; set; }
    public string Judul { get; set; } = string.Empty;
    public string Deskripsi { get; set; } = string.Empty;
    public string? FotoUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? AlamatLengkap { get; set; }
    public string Status { get; set; } = "menunggu";
    public string? Kategori { get; set; }   // ← TAMBAH
    public bool IsSynced { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // Navigation
    public User? User { get; set; }
    public Admin? Admin { get; set; }
    public ICollection<RiwayatSambat> Riwayats { get; set; } = [];  // ← TAMBAH
}


public class Gawat
{
    public Guid IdGawat { get; set; }
    public Guid? IdUser { get; set; }
    public Guid? IdAdmin { get; set; }
    public string JenisDarurat { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Status { get; set; } = "Mencari Bantuan";
    public bool IsSynced { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User? User { get; set; }
    public Admin? Admin { get; set; }
}

public class WoroWoro
{
    public Guid IdWoro { get; set; }
    public Guid? IdAdmin { get; set; }
    public string Judul { get; set; } = string.Empty;
    public string Konten { get; set; } = string.Empty;
    public string? Kategori { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Admin? Admin { get; set; }
}

public class RiwayatSambat
{
    public Guid IdRiwayat { get; set; }
    public Guid IdSambat { get; set; }
    public Guid? IdAdmin { get; set; }
    public string StatusLama { get; set; } = string.Empty;
    public string StatusBaru { get; set; } = string.Empty;
    public string? Catatan { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Navigation
    public Sambat? Sambat { get; set; }
    public Admin? Admin { get; set; }
}
