using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;
public class RestaurantDbContext: DbContext
{
    //tymczasowo ustanowienie localStringa
    private string _connectionString = 
        "Server=(localdb)\\mssqllocaldb;Database=RestaurantDb;Trusted_Connection=True";
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Dish> Dishes { get; set; }

    //możliwość dodatkowego konfigurowania pól
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(25);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Name)
            .IsRequired();
    }

    //ustawienie użycia sql server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
