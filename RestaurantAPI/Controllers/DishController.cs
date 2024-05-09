using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI;

//ścieżka zawiera id konkretnej restauracji żeby danie było z nim powiązane
[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }
    //należy pamiętać że nazwa parametru musi się pokrywać z nazwą w ścieżce
    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto) 
    {
        var newDishId = _dishService.Create(restaurantId, dto);

        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
    }

    [HttpGet("{dishId}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        DishDto dish = _dishService.GetById(restaurantId, dishId); 
        return Ok(dish);
    }

    [HttpGet]
    public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
    {
        List<DishDto> dishDtos = _dishService.GetAll(restaurantId); 
        return Ok(dishDtos);
    }
}
