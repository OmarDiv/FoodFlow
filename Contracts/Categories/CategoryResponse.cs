namespace FoodFlow.Contracts.Categories
{
    public record CategoryResponse(
        int Id,
        string Name,
        bool IsAvailable
    );

}
