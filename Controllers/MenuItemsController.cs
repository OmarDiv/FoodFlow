[ApiController]
[Route("api/restaurants/{restaurantId}/categories/{categoryId}/[controller]")]
public class MenuItemsController(IMenuItemService menuItemService) : ControllerBase
{
    private readonly IMenuItemService _menuItemService = menuItemService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.GetAllItemsAsync(restaurantId, categoryId, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetById([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.GetItemByIdAsync(restaurantId, categoryId, itemId, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromBody] CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.CreateItemAsync(restaurantId, categoryId, request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { restaurantId, categoryId, itemId = result.Value!.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, [FromBody] UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.UpdateItemAsync(restaurantId, categoryId, itemId, request, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> Delete([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.DeleteItemAsync(restaurantId, categoryId, itemId, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{itemId}/toggle-available")]
    public async Task<IActionResult> ToggleAvailability([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.ToggleAvaliableItemAsync(restaurantId, categoryId, itemId, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}