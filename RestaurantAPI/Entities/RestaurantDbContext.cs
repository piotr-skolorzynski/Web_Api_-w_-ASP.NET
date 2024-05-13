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
    //dodanie tabeli z użytkownikami
    public DbSet<User> Users { get; set; }
    //dodanie tabeli z rolami użytkowników
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //dodannie wymaganis posiadania emaila przez użytkownika
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        //wymaganie posiadania nazwy roli użytkownika
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(25);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Name)
            .IsRequired();
        
        modelBuilder.Entity<Address>()
            .Property(a => a.City)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Address>()
            .Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(50);
    }

    //ustawienie użycia sql server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
