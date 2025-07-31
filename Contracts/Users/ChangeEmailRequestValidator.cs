namespace FoodFlow.Contracts.Users
{
    public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
    {
        public ChangeEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
