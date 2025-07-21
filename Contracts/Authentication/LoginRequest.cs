namespace FoodFlow.Contracts.Athentication
{
    public record LoginRequest(
      string Email,
      string Password
    );

}

