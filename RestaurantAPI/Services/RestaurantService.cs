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
    public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public bool Update(int id, UpdateRestaurantDto dto)
    {
        //pobierz restaurację z bazy
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);
        
        //jeśli nie istnieje to zwróć false
        if (restaurant == null) return false;

        //jeśli jest to nadpisz     
        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;
        //zapisz zmiany w bazie
        _dbContext.SaveChanges();

        //zwróć info że się udało
        return true;
    }

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
