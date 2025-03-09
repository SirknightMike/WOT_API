using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using wot_api.Entities;

namespace wot_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantScore> ParticipantsScore { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users
            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique(true);

            // Competition and Participants
            modelBuilder.Entity<Competition>()
                 .HasMany(c => c.Participants)
                 .WithOne(p => p.Competition) // Added navigation property for Participant
                 .OnDelete(DeleteBehavior.Cascade);

            // Competition and Matches
            modelBuilder.Entity<Competition>()
                .HasMany(c => c.Matches)
                .WithOne(m => m.Competition)
                .HasForeignKey(m => m.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Match and ParticipantScores
            modelBuilder.Entity<Match>()
                .HasMany(m => m.ParticipantScores)
                .WithOne(ps => ps.Match)
                .HasForeignKey(ps => ps.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            // Participant and ParticipantScore
            modelBuilder.Entity<ParticipantScore>()
                .HasOne(ps => ps.Participant)
                .WithMany(p => p.ParticipantScores) // Add navigation property in Participant if missing
                .HasForeignKey(ps => ps.ParticipantId)
                .OnDelete(DeleteBehavior.NoAction);

            // Roles and Participants
            modelBuilder.Entity<Roles>()
                .ToTable("Roles")
                .HasMany(r => r.Participants)
                .WithOne(p => p.Roles)
                .HasForeignKey(p => p.RoleID);


        }
    }
}
