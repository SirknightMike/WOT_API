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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(p => p.Email)
                .IsUnique(true);
        }
    }
}
