//[Route("api/[controller]")]
//[ApiController]
//public class MenuItemsController : ControllerBase
//{
//    [HttpGet]
//    public async Task<ActionResult> GetAll(int restaurantId)
//    {
//        // Returns all menu items for a specific restaurant
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<MenuItemResponse>> GetById(int id)
//    {
//        // Returns a specific menu item
//    }

//    [HttpPost]
//    ///[Authorize(Roles = "RestaurantOwner")]
//    public async Task<ActionResult<MenuItemResponse>> Create(CreateMenuItemRequest request)
//    {
//        // Creates a new menu item
//    }

//    [HttpPut("{id}")]
//    //[Authorize(Roles = "RestaurantOwner")]
//    public async Task<IActionResult> Update(int id, UpdateMenuItemRequest request)
//    {
//        // Updates a menu item
//    }

//    [HttpDelete("{id}")]
//    //[Authorize(Roles = "RestaurantOwner")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        // Deletes a menu item
//    }

//}