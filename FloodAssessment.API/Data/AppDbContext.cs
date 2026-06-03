using Microsoft.EntityFrameworkCore;
using FloodAssessment.API.Models;

namespace FloodAssessment.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FarmAssignment> FarmAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Assessment
            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClientId).IsRequired();
                entity.HasIndex(e => e.ClientId).IsUnique();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Photo
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Assessment)
                      .WithMany(a => a.Photos)
                      .HasForeignKey(e => e.AssessmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PinHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
            });

            // FarmAssignment
            modelBuilder.Entity<FarmAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.AssignedTo)
                      .WithMany(u => u.AssignedFarms)
                      .HasForeignKey(e => e.AssignedToUserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Assessment)
                      .WithOne(a => a.FarmAssignment)
                      .HasForeignKey<FarmAssignment>(e => e.AssessmentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}