using Microsoft.EntityFrameworkCore;

namespace DeployTest;

public sealed class AppDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}