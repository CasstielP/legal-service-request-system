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
    }
}