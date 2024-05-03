namespace RestaurantAPI;
public class RestaurantSeeder
{   
    // prywatne pole przechowujące kontekst bazy danych
    private readonly RestaurantDbContext _dbContext;
    //konstruktor przez który zostanie wstrzyknięty kontekst bazy danych
    public RestaurantSeeder(RestaurantDbContext dbContext) 
    {
        _dbContext = dbContext;
    }
    //metoda odpowiedzialan na spopulowanie bazy danych
    public void Seed()
    {
        //sprawdzenie czy jesteśmy połączeni z bazą
        if(_dbContext.Database.CanConnect())
        {
           //sprawdzenie czy tabela z restauracjami jest pusta
           if(!_dbContext.Restaurants.Any())
           {
                //dodanie do bazy
                var restaurants = GetRestaurants();
                _dbContext.Restaurants.AddRange(restaurants);
                //zapisanie zmian
                _dbContext.SaveChanges();
           }
        }

    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>()
        {
            new Restaurant()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Luisville, Kentucky, that  specializes in fried chicken",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Nashville Hot Chicken",
                        Price = 10.30M
                    },

                    new Dish()
                    {
                        Name = "Chicken Nuggets",
                        Price = 5.30M
                    }
                },
                Address = new Address()
                {
                    City = "Kraków",
                    Street = "Długa 5",
                    PostalCode = "30-001"
                }
            },
            new Restaurant()
            {
                Name = "McDonald",
                Category = "Fast Food",
                Description = "McDonald's Corporation - an American chain of fast food restaurants, selling mainly burgers, fries and drinks",
                ContactEmail = "contact@mcd.com",
                HasDelivery = false,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Cheesburger",
                        Price = 3.5M
                    },

                    new Dish()
                    {
                        Name = "McDouble",
                        Price = 6.7M
                    }
                },
                Address = new Address()
                {
                    City = "Gdynia",
                    Street = "Morska 33",
                    PostalCode = "81-074"
                }
            }
        };
    }
}
