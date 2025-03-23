using Microsoft.EntityFrameworkCore;

namespace Tmp1CService.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Position> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Clients>(entity =>
        {
            entity.HasIndex(u => u.Login).IsUnique();
        });
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(9);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
            entity.HasOne(e => e.Post)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PostId);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Code).IsRequired().HasMaxLength(9);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(25);
        });*/
    }
}