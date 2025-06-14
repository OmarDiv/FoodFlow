[ApiController]
[Route("api/restaurants/{restaurantId}/categories/{categoryId}/[controller]")]
public class MenuItemsController(IMenuItemService menuItemService) : ControllerBase
{
    private readonly IMenuItemService _menuItemService = menuItemService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
    {
        var items = await _menuItemService.GetAllItemsAsync(restaurantId, categoryId, cancellationToken);
        if (items == null)
            return NotFound();
        return Ok(items);
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetById([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var item = await _menuItemService.GetItemByIdAsync(restaurantId, categoryId, itemId, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromBody] CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var createdItem = await _menuItemService.CreateItemAsync(restaurantId, categoryId, request, cancellationToken);
        if (createdItem is null)
            return BadRequest();
        return CreatedAtAction(nameof(GetById),
            new { restaurantId, categoryId, itemId = createdItem!.Id },
            createdItem);
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, [FromBody] UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var updated = await _menuItemService.UpdateItemAsync(restaurantId, categoryId, itemId, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> Delete([FromRoute] int restaurantId,[FromRoute] int categoryId,[FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var deleted = await _menuItemService.DeleteItemAsync(restaurantId, categoryId, itemId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
    [HttpPut("{itemId}/toggle-available")]
    public async Task<IActionResult> ToggleAvailability([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var toggled = await _menuItemService.ToggleAvaliableItemAsync(restaurantId, categoryId, itemId, cancellationToken);
        return toggled ? NoContent() : NotFound();
    }


}
