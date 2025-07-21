namespace FoodFlow.Contracts.Athentication
{
    public record RegisterRequest(
      string Email,
      string Password,
      string FirstName,
      string LastName
    );
}


