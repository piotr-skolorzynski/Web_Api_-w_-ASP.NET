using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI;
[Route("api/restaurant")]
public class RestaurantController: ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    public RestaurantController(IRestaurantService restaurantService) {
        _restaurantService = restaurantService;
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var isUpdated = _restaurantService.Update(id, dto);
        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var isRestaurantDeleted = _restaurantService.Delete(id);

        if (isRestaurantDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var restaurantId = _restaurantService.Create(dto);

        return Created($"/api/restaurant/{restaurantId}", null);
    }

    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurantsDtos = _restaurantService.GetAll();

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<Restaurant> Get([FromRoute] int id)
    {   
        var restaurantDto = _restaurantService.GetById(id);

        if (restaurantDto is null)
        {
            return NotFound();
        }

        return Ok(restaurantDto);
    }
}
