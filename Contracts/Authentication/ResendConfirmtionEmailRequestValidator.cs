namespace FoodFlow.Contracts.Authentication
{
    public class ResendConfirmtionEmailRequestValidator : AbstractValidator<ResendConfirmtionEmailRequest>
    {
        public ResendConfirmtionEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
