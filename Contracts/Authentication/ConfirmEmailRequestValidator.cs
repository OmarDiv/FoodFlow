namespace FoodFlow.Contracts.Authentication
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty();
            RuleFor(x => x.code)
                .NotEmpty();

        }
    }
}
