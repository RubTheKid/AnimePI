using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Infra.Context;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new UserMapping());
        mb.ApplyConfiguration(new FavoriteMapping());
        mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}