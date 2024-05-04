using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    IEnumerable<RestaurantDto> GetAll();
    int Create(CreateRestaurantDto dto);
    bool Delete(int id);
}
public class RestaurantService: IRestaurantService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    //metoda do usuwania zasobu z bazy danych do wykorzystania w kontrolerze
    public bool Delete(int id)
    {
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id); 

        if (restaurant is null) return false;

        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();

        return true;      
    }

    public RestaurantDto GetById(int id)
    {
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id); 
        
        if (restaurant == null) return null;

        var result = _mapper.Map<RestaurantDto>(restaurant);

        return result;
    }

    public IEnumerable<RestaurantDto> GetAll() 
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToList();
        
        var result = _mapper.Map<List<RestaurantDto>>(restaurants);

        return result;
    }

    public int Create(CreateRestaurantDto dto)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant.Id;
    }
}
