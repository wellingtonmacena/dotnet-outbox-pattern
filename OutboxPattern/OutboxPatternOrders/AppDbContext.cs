using Microsoft.EntityFrameworkCore;
using OutboxPattern.Infrastructure.Configurations;
using System.Diagnostics.CodeAnalysis;

namespace OutboxPatternOrders;

[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrdersOutboxMessage> OrdersOutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new OrderOutboxConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration()); // já existente
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntries = ChangeTracker.Entries().Where(e => e is { State: EntityState.Modified, Entity: Entity });
        foreach (var entry in modifiedEntries) ((Entity)entry.Entity).MarkAsUpdated();
        return base.SaveChangesAsync(cancellationToken);
    }
}
