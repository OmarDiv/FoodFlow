using FoodFlow.Contracts.Authentication;

namespace FoodFlow.Contracts.Athentication
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();

        }


    }
}
