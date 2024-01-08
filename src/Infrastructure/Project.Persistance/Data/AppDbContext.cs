using Microsoft.EntityFrameworkCore;
using Project.Domain.Accounts;
using Project.Domain.Logs;
using Project.Domain.ATMTransactions;
using Project.Domain.Users;

namespace Project.Persistance.Data;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ATMTransaction> ATMTransactions { get; set; }
    public DbSet<AuditLog> Logs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ATMTransaction>()
           .Property(transaction => transaction.Amount)
           .HasPrecision(18, 2);

        modelBuilder.Entity<Account>()
            .Property(account => account.Balance)
            .HasPrecision(18, 2);
    }
}
