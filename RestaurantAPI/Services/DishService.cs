using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;
public interface IDishService
{
    int Create(int restaurantId, CreateDishDto dto);
    DishDto GetById(int restaurantId, int dishId);
    List<DishDto> GetAll(int restaurantId);
}
public class DishService : IDishService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    public DishService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public int Create(int restaurantId, CreateDishDto dto)
    {
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var dishEntity = _mapper.Map<Dish>(dto);
        dishEntity.RestaurantId = restaurantId;

        _dbContext.Dishes.Add(dishEntity);
        _dbContext.SaveChanges();

        return dishEntity.Id; 
    }

    public DishDto GetById(int restaurantId, int dishId)
    {
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);
        //sprawdzenie czy danie istnieje lub czy istnieje w kontekscie przekazanej restauracji
        if (dish is null || dish.RestaurantId != restaurantId)
        {
            throw new NotFoundException("Dish not found");
        }

        var dishDto = _mapper.Map<DishDto>(dish); //mapper ma taką mapę

        return dishDto;
    } 

    public List<DishDto> GetAll(int restaurantId)
    {
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Dishes) //dla tej restauracji chcemy załączyć również wszystkie jej dania
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);
        return dishDtos;
    }
}
