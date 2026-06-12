# Aduin Jember — Backend API

Backend API untuk aplikasi **Aduin Jember**, platform pelaporan digital masyarakat untuk Smart City Kabupaten Jember. Dibangun dengan **ASP.NET Core 8 Web API** dan **Supabase** (PostgreSQL, Auth, Storage).

🔗 **Live API:** https://aduinjember-production.up.railway.app

---

## 📋 Tech Stack

| Komponen | Teknologi |
|---|---|
| Backend Framework | ASP.NET Core 8 Web API |
| Database | PostgreSQL (Supabase) |
| ORM | Entity Framework Core |
| Autentikasi | Supabase Auth (JWT) |
| Dokumentasi API | Swagger / OpenAPI |
| Hosting | Railway |
| Arsitektur | Layered (Controller → Service → Repository) |

---

## 🏗️ Struktur Project

```
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
```

---

## 🔑 Fitur & Role

### User / Masyarakat
- Registrasi & login
- Membuat laporan (**Sambat**) dengan kategori: Sosial, Infrastruktur, Layanan Umum
- Upload foto bukti laporan
- Kirim lokasi otomatis (Maps API)
- Laporan darurat (**Gawat**): Kecelakaan, Kriminal, Bencana
- Melihat status & riwayat laporan
- Melihat informasi publik (**Woro-Woro**)
- Kelola profil

### Admin
- Login dashboard admin
- Melihat & memverifikasi semua laporan
- Update status laporan (menunggu → diproses → ditindaklanjuti → selesai)
- Filter laporan & riwayat berdasarkan kategori
- Hapus laporan spam
- Kelola informasi publik (Woro-Woro)
- Pantau laporan darurat
- Kelola data pengguna

---

## 🚀 Menjalankan Secara Lokal

### Prasyarat
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Project Supabase (sudah dikonfigurasi)

### Langkah

1. Clone repository
   ```bash
   git clone https://github.com/fahminazmuddin77/AduinJember.git
   cd AduinJember
   ```

2. Isi `appsettings.json` dengan kredensial Supabase
   ```json
   {
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
   ```

3. Restore dependencies & jalankan
   ```bash
   dotnet restore
   dotnet run --urls "http://localhost:5050"
   ```

4. Buka Swagger UI
   ```
   http://localhost:5050
   ```

---

## 📡 Endpoint Utama

| Method | Endpoint | Akses | Deskripsi |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Registrasi akun baru |
| POST | `/api/auth/login` | Public | Login user/admin |
| POST | `/api/sambat` | User | Kirim laporan baru |
| GET | `/api/sambat` | User/Admin | Lihat laporan (filter status & kategori) |
| PATCH | `/api/sambat/{id}/status` | Admin | Update status laporan |
| DELETE | `/api/sambat/{id}` | Admin | Hapus laporan |
| GET | `/api/sambat/riwayat` | Admin | Riwayat perubahan status (filter kategori) |
| GET | `/api/sambat/{id}/riwayat` | User/Admin | Riwayat per laporan |
| POST | `/api/gawat` | User | Kirim laporan darurat |
| GET | `/api/gawat` | User/Admin | Lihat laporan darurat |
| PATCH | `/api/gawat/{id}/status` | Admin | Update status penanganan |
| GET | `/api/woro-woro` | User/Admin | Lihat informasi publik |
| POST | `/api/woro-woro` | Admin | Tambah informasi |
| PATCH | `/api/woro-woro/{id}` | Admin | Edit informasi |
| DELETE | `/api/woro-woro/{id}` | Admin | Hapus informasi |
| GET | `/api/profil` | User/Admin | Lihat profil sendiri |
| PATCH | `/api/profil` | User | Update profil |
| GET | `/api/profil/users` | Admin | Lihat semua user |

Dokumentasi interaktif lengkap tersedia di Swagger UI (`/`).

---

## 🗄️ Skema Database

5 tabel utama di Supabase PostgreSQL:

- **users** — data masyarakat pengguna aplikasi
- **admins** — data administrator
- **sambat** — laporan masyarakat (dengan kategori & status)
- **gawat** — laporan darurat
- **woro_woro** — informasi publik dari admin
- **riwayat_sambat** — log perubahan status laporan

Semua tabel dilindungi **Row Level Security (RLS)** — user hanya bisa akses data miliknya, admin memiliki akses penuh.

---

## 🔐 Autentikasi

API menggunakan **Supabase JWT**. Setelah login, sertakan token pada header:

```
Authorization: Bearer <access_token>
```

Role ditentukan otomatis: jika UID pengguna terdaftar di tabel `admins`, maka diperlakukan sebagai admin.

---

## 👥 Developer API — Kelompok 13

| Nama | NIM |
|---|---|
| Mohammad Fahmi Nazmuddin | 242410102011 |


**Program Studi Teknologi Informasi — Universitas Jember (2026)**
