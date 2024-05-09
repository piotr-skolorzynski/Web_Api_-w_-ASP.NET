﻿using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpDelete]
    public ActionResult DeleteAll([FromRoute] int restaurantId)
    {
        _dishService.RemoveAll(restaurantId);
        return NoContent();
    }

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
