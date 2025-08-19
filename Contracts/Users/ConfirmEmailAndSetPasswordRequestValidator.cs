namespace FoodFlow.Contracts.Users
{
    public class ConfirmEmailAndSetPasswordRequestValidator : AbstractValidator<ConfirmEmailAndSetPasswordRequest>
    {
        public ConfirmEmailAndSetPasswordRequestValidator()
        {

            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotNull();



        }
    }
}
