using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    IEnumerable<RestaurantDto> GetAll();
    int Create(CreateRestaurantDto dto);
    bool Delete(int id);
    bool Update(int id, UpdateRestaurantDto dto);
}
public class RestaurantService: IRestaurantService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public bool Update(int id, UpdateRestaurantDto dto)
    {
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);
        
        if (restaurant == null) return false;

        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;
        _dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        //przykład wykorzystania loggera do zapisu histori usuwania encji restauracji
        _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

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
