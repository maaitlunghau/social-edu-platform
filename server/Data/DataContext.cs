using Microsoft.EntityFrameworkCore;
using shared;
using shared.Domain;

namespace server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<RefreshTokenRecords> RefreshTokenRecords { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAtUTC = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUTC = DateTime.UtcNow;
                entry.Entity.UpdatedAtUTC = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(ct);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Mai Trung Hau",
                Email = "trunghau@mstsoftware.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("admin@123"),
                Role = UserRole.Admin,
                Status = UserStatus.Active,
                IsEmailVerified = true,
                Avatar = string.Empty,
                Phone = string.Empty,
                CreatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "John Doe",
                Email = "john@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = UserRole.User,
                Status = UserStatus.Active,
                IsEmailVerified = true,
                Avatar = string.Empty,
                Phone = string.Empty,
                CreatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Jane Smith",
                Email = "jane@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = UserRole.Educator,
                Status = UserStatus.Active,
                IsEmailVerified = true,
                Avatar = string.Empty,
                Phone = string.Empty,
                CreatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Bob Johnson",
                Email = "bob@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = UserRole.User,
                Status = UserStatus.Pending,
                IsEmailVerified = false,
                Avatar = string.Empty,
                Phone = string.Empty,
                CreatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Alice Williams",
                Email = "alice@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = UserRole.User,
                Status = UserStatus.Banned,
                IsEmailVerified = true,
                Avatar = string.Empty,
                Phone = string.Empty,
                CreatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUTC = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<RefreshTokenRecords>()
            .HasOne(rft => rft.User)
            .WithMany(u => u.RefreshTokenRecords)
            .HasForeignKey(rft => rft.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshTokenRecords>()
            .HasIndex(rft => rft.UserId);

        modelBuilder.Entity<RefreshTokenRecords>()
            .HasIndex(rft => rft.RefreshToken);

        modelBuilder.Entity<RefreshTokenRecords>()
            .HasIndex(rft => rft.AccessTokenJTI);
    }
}
