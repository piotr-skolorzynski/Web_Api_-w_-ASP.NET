using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI;
[Route("api/restaurant")]
public class RestaurantController: ControllerBase
{
    //żeby móc pobrać dane z bazy musimy mieć dostęp do jej kontekstu
    private readonly RestaurantDbContext _dbContext;
    public RestaurantController(RestaurantDbContext dbContext) {
        _dbContext = dbContext;
    }
    //metoda do pobrania wszystkich restauracji, domyślnie atrybut jest HttpGet 
    // ale dobrą praktyką jest zawsze dodanie atrubutu
    [HttpGet]
    public ActionResult<IEnumerable<Restaurant>> GetAll()
    {
        var restaurants = _dbContext
            .Restaurants
            .ToList();

        return Ok(restaurants);
    }
    //zwrócenie konkretnej restauracji na bazie id przekazanego w ścieżce
    [HttpGet("{id}")] // api/restaurant/id
    public ActionResult<Restaurant> Get([FromRoute] int id)
    {   
        //znajdź w bazie konkretną restaurację
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id); 

        //jeśli restauracja nieistnieje to rzuć błędem 404
        if (restaurant is null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }
}
