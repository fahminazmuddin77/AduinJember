using Microsoft.EntityFrameworkCore;
using AduinJember.Models;

namespace AduinJember.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Sambat> Sambats => Set<Sambat>();
    public DbSet<Gawat> Gawats => Set<Gawat>();
    public DbSet<WoroWoro> WoroWoros => Set<WoroWoro>();
    public DbSet<RiwayatSambat> RiwayatSambats => Set<RiwayatSambat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.IdUser);
            e.Property(x => x.IdUser).HasColumnName("id_user");
            e.Property(x => x.Nama).HasColumnName("nama").IsRequired();
            e.Property(x => x.Email).HasColumnName("email").IsRequired();
            e.Property(x => x.Nik).HasColumnName("nik");
            e.Property(x => x.FotoProfil).HasColumnName("foto_profil");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Admin>(e =>
        {
            e.ToTable("admins");
            e.HasKey(x => x.IdAdmin);
            e.Property(x => x.IdAdmin).HasColumnName("id_admin");
            e.Property(x => x.Nama).HasColumnName("nama").IsRequired();
            e.Property(x => x.Email).HasColumnName("email").IsRequired();
            e.Property(x => x.Instansi).HasColumnName("instansi");
            e.Property(x => x.Jabatan).HasColumnName("jabatan");
            e.Property(x => x.FotoProfil).HasColumnName("foto_profil");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Sambat>(e =>
        {
            e.ToTable("sambat");
            e.HasKey(x => x.IdSambat);
            e.Property(x => x.IdSambat).HasColumnName("id_sambat");
            e.Property(x => x.IdUser).HasColumnName("id_user");
            e.Property(x => x.IdAdmin).HasColumnName("id_admin");
            e.Property(x => x.Judul).HasColumnName("judul").IsRequired();
            e.Property(x => x.Deskripsi).HasColumnName("deskripsi").IsRequired();
            e.Property(x => x.FotoUrl).HasColumnName("foto_url");
            e.Property(x => x.Latitude).HasColumnName("latitude");
            e.Property(x => x.Longitude).HasColumnName("longitude");
            e.Property(x => x.AlamatLengkap).HasColumnName("alamat_lengkap");
            e.Property(x => x.Status).HasColumnName("status").HasDefaultValue("menunggu");
            e.Property(x => x.Kategori).HasColumnName("kategori");
            e.Property(x => x.IsSynced).HasColumnName("is_synced");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(x => x.User).WithMany(u => u.Sambats)
                .HasForeignKey(x => x.IdUser).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Admin).WithMany(a => a.Sambats)
                .HasForeignKey(x => x.IdAdmin).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Gawat>(e =>
        {
            e.ToTable("gawat");
            e.HasKey(x => x.IdGawat);
            e.Property(x => x.IdGawat).HasColumnName("id_gawat");
            e.Property(x => x.IdUser).HasColumnName("id_user");
            e.Property(x => x.IdAdmin).HasColumnName("id_admin");
            e.Property(x => x.JenisDarurat).HasColumnName("jenis_darurat").IsRequired();
            e.Property(x => x.Latitude).HasColumnName("latitude");
            e.Property(x => x.Longitude).HasColumnName("longitude");
            e.Property(x => x.Status).HasColumnName("status").HasDefaultValue("Mencari Bantuan");
            e.Property(x => x.IsSynced).HasColumnName("is_synced");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.User).WithMany(u => u.Gawats)
                .HasForeignKey(x => x.IdUser).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Admin).WithMany(a => a.Gawats)
                .HasForeignKey(x => x.IdAdmin).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<WoroWoro>(e =>
        {
            e.ToTable("woro_woro");
            e.HasKey(x => x.IdWoro);
            e.Property(x => x.IdWoro).HasColumnName("id_woro");
            e.Property(x => x.IdAdmin).HasColumnName("id_admin");
            e.Property(x => x.Judul).HasColumnName("judul").IsRequired();
            e.Property(x => x.Konten).HasColumnName("konten").IsRequired();
            e.Property(x => x.Kategori).HasColumnName("kategori");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.Admin).WithMany(a => a.WoroWoros)
                .HasForeignKey(x => x.IdAdmin).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RiwayatSambat>(e =>
        {
            e.ToTable("riwayat_sambat");
            e.HasKey(x => x.IdRiwayat);
            e.Property(x => x.IdRiwayat).HasColumnName("id_riwayat");
            e.Property(x => x.IdSambat).HasColumnName("id_sambat");
            e.Property(x => x.IdAdmin).HasColumnName("id_admin");
            e.Property(x => x.StatusLama).HasColumnName("status_lama").IsRequired();
            e.Property(x => x.StatusBaru).HasColumnName("status_baru").IsRequired();
            e.Property(x => x.Catatan).HasColumnName("catatan");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.Sambat).WithMany(s => s.Riwayats)
                .HasForeignKey(x => x.IdSambat).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Admin).WithMany()
                .HasForeignKey(x => x.IdAdmin).OnDelete(DeleteBehavior.SetNull);
        });
    }
}