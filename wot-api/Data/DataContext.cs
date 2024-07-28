using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using wot_api.Entities;

namespace wot_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantScore> ParticipantsScore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(p => p.Email)
                .IsUnique(true);

            modelBuilder.Entity<Competition>()
                 .HasMany(c => c.Participants)
                 .WithOne()
                 .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Competition and Match
            modelBuilder.Entity<Competition>()
                .HasMany(c => c.Matches)
                .WithOne(m => m.Competition)
                .HasForeignKey(m => m.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Match and ParticipantScore
            modelBuilder.Entity<Match>()
                .HasMany(m => m.ParticipantScores)
                .WithOne(ps => ps.Match)
                .HasForeignKey(ps => ps.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Participant and ParticipantScore with NoAction on delete
            modelBuilder.Entity<ParticipantScore>()
                .HasOne(ps => ps.Participant)
                .WithMany()
                .HasForeignKey(ps => ps.ParticipantId)
                .OnDelete(DeleteBehavior.NoAction);

        
        }
    }
}
