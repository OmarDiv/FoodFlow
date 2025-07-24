

namespace FoodFlow.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error NotFound = new("Category.NotFound", "The requested category was not found.", StatusCodes.Status404NotFound);
        public static readonly Error RestaruantOrCategoryNotFound = new("Category.RestaruantOrCategoryNotFound", "Restaruant Or Category NotFound.", StatusCodes.Status404NotFound);
        public static readonly Error NoCategoriesFound = new("Category.NoCategoriesFound", "No categories were found for the specified restaurant.", StatusCodes.Status404NotFound);
        public static readonly Error AlreadyExists = new("Category.AlreadyExists", "A category with the same name already exists in this restaurant.", StatusCodes.Status409Conflict);
        public static readonly Error FailedToCreate = new("Category.FailedToCreate", "Failed to create the category.", StatusCodes.Status500InternalServerError);
        public static readonly Error FailedToUpdate = new("Category.FailedToUpdate", "Failed to update the category.", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToDelete = new("Category.FailedToDelete", "Failed to delete the category.", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToToggleStatus = new("Category.FailedToToggleStatus", "Failed to change the category status.", StatusCodes.Status400BadRequest);
    }
}

