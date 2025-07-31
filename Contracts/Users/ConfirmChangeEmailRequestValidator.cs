namespace FoodFlow.Contracts.users
{
    public class ConfirmChangeEmailRequestValidator : AbstractValidator<ConfirmChangeEmailRequest>
    {
        public ConfirmChangeEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.UserId)
                .NotEmpty();
            RuleFor(x => x.Code)
                .NotEmpty();

        }
    }
}
