Aduin Jember — Backend API

Backend API untuk aplikasi Aduin Jember, platform pelaporan digital masyarakat untuk Smart City Kabupaten Jember. Dibangun dengan ASP.NET Core 8 Web API dan Supabase (PostgreSQL, Auth, Storage).

🔗 Live API: https://aduinjember-production.up.railway.app


📋 Tech Stack

KomponenTeknologiBackend FrameworkASP.NET Core 8 Web APIDatabasePostgreSQL (Supabase)ORMEntity Framework CoreAutentikasiSupabase Auth (JWT)Dokumentasi APISwagger / OpenAPIHostingRailwayArsitekturLayered (Controller → Service → Repository)


🏗️ Struktur Project

AduinJember/
├── Controllers/        # HTTP endpoints
│   ├── AuthController.cs
│   ├── SambatController.cs
│   ├── GawatController.cs
│   ├── WoroWoroController.cs
│   └── ProfilController.cs
├── Services/            # Business logic
├── Repositories/        # Data access layer (EF Core)
├── Models/               # Entity classes
├── DTOs/                 # Request & response objects
├── Middleware/           # JWT validation, error handling
├── Configuration/        # DbContext & settings
└── Program.cs


🔑 Fitur & Role

User / Masyarakat


Registrasi & login
Membuat laporan (Sambat) dengan kategori: Sosial, Infrastruktur, Layanan Umum
Upload foto bukti laporan
Kirim lokasi otomatis (Maps API)
Laporan darurat (Gawat): Kecelakaan, Kriminal, Bencana
Melihat status & riwayat laporan
Melihat informasi publik (Woro-Woro)
Kelola profil


Admin


Login dashboard admin
Melihat & memverifikasi semua laporan
Update status laporan (menunggu → diproses → ditindaklanjuti → selesai)
Filter laporan & riwayat berdasarkan kategori
Hapus laporan spam
Kelola informasi publik (Woro-Woro)
Pantau laporan darurat
Kelola data pengguna



🚀 Menjalankan Secara Lokal

Prasyarat


.NET 8 SDK
Project Supabase (sudah dikonfigurasi)


Langkah


Clone repository


bash   git clone https://github.com/fahminazmuddin77/AduinJember.git
   cd AduinJember


Isi appsettings.json dengan kredensial Supabase


json   {
     "Supabase": {
       "Url": "https://xxxxx.supabase.co",
       "AnonKey": "your-anon-key",
       "ServiceRoleKey": "your-service-role-key",
       "JwtSecret": "your-jwt-secret"
     },
     "ConnectionStrings": {
       "DefaultConnection": "Host=...;Port=6543;Database=postgres;Username=...;Password=...;SSL Mode=Require"
     }
   }


Restore dependencies & jalankan


bash   dotnet restore
   dotnet run --urls "http://localhost:5050"


Buka Swagger UI


   http://localhost:5050


📡 Endpoint Utama

MethodEndpointAksesDeskripsiPOST/api/auth/registerPublicRegistrasi akun baruPOST/api/auth/loginPublicLogin user/adminPOST/api/sambatUserKirim laporan baruGET/api/sambatUser/AdminLihat laporan (filter status & kategori)PATCH/api/sambat/{id}/statusAdminUpdate status laporanDELETE/api/sambat/{id}AdminHapus laporanGET/api/sambat/riwayatAdminRiwayat perubahan status (filter kategori)GET/api/sambat/{id}/riwayatUser/AdminRiwayat per laporanPOST/api/gawatUserKirim laporan daruratGET/api/gawatUser/AdminLihat laporan daruratPATCH/api/gawat/{id}/statusAdminUpdate status penangananGET/api/woro-woroUser/AdminLihat informasi publikPOST/api/woro-woroAdminTambah informasiPATCH/api/woro-woro/{id}AdminEdit informasiDELETE/api/woro-woro/{id}AdminHapus informasiGET/api/profilUser/AdminLihat profil sendiriPATCH/api/profilUserUpdate profilGET/api/profil/usersAdminLihat semua user

Dokumentasi interaktif lengkap tersedia di Swagger UI (/).


🗄️ Skema Database

5 tabel utama di Supabase PostgreSQL:


users — data masyarakat pengguna aplikasi
admins — data administrator
sambat — laporan masyarakat (dengan kategori & status)
gawat — laporan darurat
woro_woro — informasi publik dari admin
riwayat_sambat — log perubahan status laporan


Semua tabel dilindungi Row Level Security (RLS) — user hanya bisa akses data miliknya, admin memiliki akses penuh.


🔐 Autentikasi

API menggunakan Supabase JWT. Setelah login, sertakan token pada header:

Authorization: Bearer <access_token>

Role ditentukan otomatis: jika UID pengguna terdaftar di tabel admins, maka diperlakukan sebagai admin.

