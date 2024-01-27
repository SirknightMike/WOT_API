using Microsoft.EntityFrameworkCore;
using System.Data;
using wot_api.Entities;

namespace wot_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options) { }

        public DbSet<Users> Users { get; set; }
    }
}
