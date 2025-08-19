namespace FoodFlow.Contracts.Users
{
    public class ResendConfirmEmailAndSetPasswordValidator : AbstractValidator<ResendConfirmEmailAndSetPasswordRequest>
    {
        public ResendConfirmEmailAndSetPasswordValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
