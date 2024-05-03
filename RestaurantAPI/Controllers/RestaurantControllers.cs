using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;
[Route("api/restaurant")]
public class RestaurantController: ControllerBase
{
    //żeby móc pobrać dane z bazy musimy mieć dostęp do jej kontekstu
    private readonly RestaurantDbContext _dbContext;
    //instancja automappera do wykorzystania profilowania 
    private readonly IMapper _mapper;
    public RestaurantController(RestaurantDbContext dbContext, IMapper mapper) {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    //dobrą praktyką jest zawsze dodanie atrybutu GetHttp mimo że jest domślny 
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToList();
        
        // wykorzystanie zewnętrznej paczki automapper do mapowania z jednej klasy na drugą
        // zamiast rozpisywania jakie pola mają być przekazana a jakie nie
        var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

        return Ok(restaurantsDtos);
    }
    //zwrócenie konkretnej restauracji na bazie id przekazanego w ścieżce
    [HttpGet("{id}")] // api/restaurant/id
    public ActionResult<Restaurant> Get([FromRoute] int id)
    {   
        //znajdź w bazie konkretną restaurację
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id); 

        //jeśli restauracja nieistnieje to rzuć błędem 404
        if (restaurant is null)
        {
            return NotFound();
        }

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        return Ok(restaurantDto);
    }
}
