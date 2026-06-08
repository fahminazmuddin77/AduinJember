using AduinJember.Configuration;
using AduinJember.Middleware;
using AduinJember.Repositories;
using AduinJember.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Settings ──────────────────────────────────────────
var supabaseSettings = builder.Configuration.GetSection("Supabase").Get<SupabaseSettings>()!;
builder.Services.Configure<SupabaseSettings>(builder.Configuration.GetSection("Supabase"));

// ── Database (EF Core + Supabase PostgreSQL) ──────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Supabase Client ───────────────────────────────────
builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        supabaseSettings.Url,
        supabaseSettings.AnonKey,
        new Supabase.SupabaseOptions { AutoConnectRealtime = false }
    ));

// ── JWT Authentication (validasi Supabase JWT) ────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = supabaseSettings.Url + "/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = supabaseSettings.Url + "/auth/v1",
            ValidateAudience = true,
            ValidAudience = "authenticated",
            RoleClaimType = "role",
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ── Repositories ──────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ISambatRepository, SambatRepository>();
builder.Services.AddScoped<IGawatRepository, GawatRepository>();
builder.Services.AddScoped<IWoroWoroRepository, WoroWoroRepository>();
builder.Services.AddScoped<IRiwayatRepository, RiwayatRepository>();  // ← TAMBAH

// ── Services ──────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISambatService, SambatService>();
builder.Services.AddScoped<IGawatService, GawatService>();
builder.Services.AddScoped<IWoroWoroService, WoroWoroService>();

// ── Controllers & Swagger ─────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aduin Jember API",
        Version = "v1",
        Description = "Backend API untuk aplikasi pelaporan digital Aduin Jember"
    });

    // Tombol Authorize di Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Masukkan JWT token dari Supabase. Contoh: Bearer eyJhbG..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ── CORS (untuk Flutter/web) ──────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ═════════════════════════════════════════════════════
var app = builder.Build();
// ═════════════════════════════════════════════════════

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aduin Jember API v1");
        c.RoutePrefix = string.Empty; // Swagger di root URL
    });
}

app.UseCors("AllowAll");

// Middleware urutan penting: Exception → JWT → Auth → Controllers
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();