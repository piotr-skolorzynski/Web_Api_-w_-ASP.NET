using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;
[Route("api/restaurant")]
public class RestaurantController: ControllerBase
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    public RestaurantController(RestaurantDbContext dbContext, IMapper mapper) {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    //atrybut FromBody określa że model restauracji przyjdzie w ciele zapytania
    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        //sprawdzenie walidacji modelu dto, który przyszedł od klienta
        if (!ModelState.IsValid)
        {
            //jeśli nie to prześlij błąd i model walidacji (będzie to konkretne pole, które nie spełnia warunków walidacji)
            return BadRequest(ModelState);
        }
        //utworzenie encji restauracji korzystając z mappera i przesłanego dto restauracji
        var restaurant = _mapper.Map<Restaurant>(dto);
        //dodanie restauracji do bazy
        _dbContext.Restaurants.Add(restaurant);
        //zapisz zmiany na kontekscie bazy danych
        _dbContext.SaveChanges();

        //po utworzeniu przesyłamy do klienta info z kodem 201 o utworzeniu i uri lokacji zasobu
        //opcjonalnie można przekazać ciało odpowiedzi ale tutaj jest null
        return Created($"/api/restaurant/{restaurant.Id}", null);
    }

    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToList();
        
        var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")] // api/restaurant/id
    public ActionResult<Restaurant> Get([FromRoute] int id)
    {   
        //znajdź w bazie konkretną restaurację
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id); 

        if (restaurant is null)
        {
            return NotFound();
        }

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        return Ok(restaurantDto);
    }
}
