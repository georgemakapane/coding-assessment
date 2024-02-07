using Assessment.Models;
using Microsoft.EntityFrameworkCore;

public class RouletteDbContext : DbContext
{
    public RouletteDbContext(DbContextOptions<RouletteDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Bet> Bet { get; set; }
    public DbSet<SpinHistory> SpinHistory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string dbPath = $"{basePath}Roulette.db";
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}
