using Microsoft.EntityFrameworkCore;
using ASPServerAPI.Models;

namespace ASPServerAPI.Data
{
    public class AppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<MonsterEntity> Monsters { get; set; }

    }
}
