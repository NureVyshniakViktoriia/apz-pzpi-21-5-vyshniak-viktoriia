using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DAL.DbContexts;
public class DbContextBase : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<UserProfile> UserProfiles { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; } = null!;

    public DbSet<Pet> Pets { get; set; } = null!;

    public DbSet<Institution> Institutions { get; set; } = null!;

    public DbSet<Facility> Facilities { get; set; } = null!;

    public DbSet<DiaryNote> DiaryNotes { get; set; } = null!;

    public DbSet<HealthRecord> HealthRecords { get; set; } = null!;

    public DbSet<Rating> Ratings { get; set; } = null!;

    public DbSet<Notification> Notifications { get; set; } = null!;

    public DbSet<RFIDSettings> RFIDSettings { get; set; } = null!;

    public DbSet<ArduinoSettings> ArduinoSettings { get; set; } = null!;

    public DbContextBase(DbContextOptions<DbContextBase> options)
        : base(options) 
    {
        Database.EnsureCreated();
    }

    public void Commit()
    {
        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapUser(modelBuilder);
        MapPet(modelBuilder);
        MapInsitution(modelBuilder);
        AddAdmin(modelBuilder);
        MapEnums(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    #region Mappings

    private void MapUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .Property(u => u.UserId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProfile>()
            .HasKey(p => p.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .Property(t => t.RefreshTokenId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<User>()
            .HasMany(u => u.ResetPasswordTokens)
            .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResetPasswordToken>()
            .Property(t => t.ResetPasswordTokenId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<User>()
           .HasMany(u => u.Pets)
           .WithOne(p => p.Owner)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
           .HasMany(u => u.Institutions)
           .WithOne(i => i.Owner)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Notification>()
            .Property(n => n.NotificationId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<User>()
           .HasMany(n => n.Notifications)
           .WithOne(u => u.User)
           .OnDelete(DeleteBehavior.NoAction);
    }

    private void MapInsitution(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Institution>()
            .HasKey(i => i.InstitutionId);

        modelBuilder.Entity<Institution>()
            .HasMany(i => i.Facilities)
            .WithMany(s => s.Institutions)
            .UsingEntity("InstitutionFacilities");

        modelBuilder.Entity<Facility>()
            .HasKey(f => f.FacilityId);

        modelBuilder.Entity<Facility>()
            .Property(f => f.FacilityId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Rating>()
            .Property(c => c.RatingId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<Institution>()
            .HasMany(i => i.Ratings)
            .WithOne(r => r.Institution)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Institution>()
            .HasOne(t => t.RFIDSettings)
            .WithOne(r => r.Institution)
            .HasForeignKey<RFIDSettings>(r => r.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RFIDSettings>()
            .HasKey(r => r.InstitutionId);
    }

    private void MapPet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>()
            .HasKey(p => p.PetId);

        modelBuilder.Entity<Pet>()
            .Property(p => p.PetId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.DiaryNotes)
            .WithOne(dn => dn.Pet)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.HealthRecords)
            .WithOne(hr => hr.Pet)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DiaryNote>()
            .HasKey(dn => dn.DiaryNoteId);

        modelBuilder.Entity<DiaryNote>()
            .Property(dn => dn.DiaryNoteId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<HealthRecord>()
            .HasKey(hr => hr.HealthRecordId);

        modelBuilder.Entity<HealthRecord>()
            .Property(hr => hr.HealthRecordId)
            .HasValueGenerator(typeof(GuidValueGenerator));

        modelBuilder.Entity<Pet>()
           .HasMany(p => p.Notifications)
           .WithOne(n => n.Pet)
           .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Pet>()
           .HasOne(p => p.ArduinoSettings)
           .WithOne(s => s.Pet)
           .HasForeignKey<ArduinoSettings>(s => s.PetId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ArduinoSettings>()
            .HasKey(s => s.PetId);
    }

    private void MapEnums(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Institution>()
            .Property(i => i.InstitutionType)
            .HasConversion<int>();

        modelBuilder.Entity<Pet>()
            .Property(p => p.PetType)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<int>();

        modelBuilder.Entity<UserProfile>()
            .Property(u => u.Gender)
            .HasConversion<int>();

        modelBuilder.Entity<UserProfile>()
            .Property(u => u.Region)
            .HasConversion<int>();
    }

    private void AddAdmin(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 1,
            Login = "Admin",
            PasswordHash = "$2a$10$WkrWKFdubfRwcY4MjdFELui7Dh8r3ykAvDYOQPvQud0vPlxFHVen.", // password: admin231_rte
            PasswordSalt = "d!W2~4~zI{wq:l<p",
            RegisteredOnUtc = DateTime.UtcNow,
            Role = Common.Enums.Role.Admin
        });

        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            UserId = 1,
            Email = "admin@pawpoint.com"
        });
    }

    #endregion
}
