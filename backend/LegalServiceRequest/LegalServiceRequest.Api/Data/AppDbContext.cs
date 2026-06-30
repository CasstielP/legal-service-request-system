using LegalServiceRequest.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LegalServiceRequest.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    public DbSet<RequestNote> RequestNotes => Set<RequestNote>();

    public DbSet<RequestStatusHistory> RequestStatusHistories => Set<RequestStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.AssignedToUser)
            .WithMany()
            .HasForeignKey(sr => sr.AssignedToUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.CreatedByUser)
            .WithMany()
            .HasForeignKey(sr => sr.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RequestNote>()
            .HasOne(rn => rn.ServiceRequest)
            .WithMany()
            .HasForeignKey(rn => rn.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RequestNote>()
            .HasOne(rn => rn.User)
            .WithMany()
            .HasForeignKey(rn => rn.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RequestStatusHistory>()
            .HasOne(rsh => rsh.ServiceRequest)
            .WithMany()
            .HasForeignKey(rsh => rsh.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RequestStatusHistory>()
            .HasOne(rsh => rsh.ChangedByUser)
            .WithMany()
            .HasForeignKey(rsh => rsh.ChangedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        FullName = "Admin User",
        Email = "admin@county.local",
        Role = "Admin",
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    },
    new User
    {
        Id = 2,
        FullName = "Staff User",
        Email = "staff@county.local",
        Role = "Staff",
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    },
    new User
    {
        Id = 3,
        FullName = "Developer User",
        Email = "developer@county.local",
        Role = "Developer",
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    }
);

        modelBuilder.Entity<ServiceRequest>().HasData(
            new ServiceRequest
            {
                Id = 1,
                Title = "Update department intranet content",
                Description = "County Counsel staff requested an update to internal policy reference content on the department intranet.",
                Department = "County Counsel",
                Priority = "Medium",
                Status = "Open",
                CreatedByUserId = 2,
                AssignedToUserId = 3,
                DueDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc)
            },
            new ServiceRequest
            {
                Id = 2,
                Title = "Investigate login issue",
                Description = "A staff user reported that they are unable to access the internal request portal after password reset.",
                Department = "Technology & Business Solutions",
                Priority = "High",
                Status = "In Progress",
                CreatedByUserId = 2,
                AssignedToUserId = 3,
                DueDate = new DateTime(2026, 7, 5, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 6, 29, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}