using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace InfrastructureLayer.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<Game> Games => Set<Game>();
    }
}