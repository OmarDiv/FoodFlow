namespace FoodFlow.Contracts.Authentication
{
    public class ResetPasswordRequestVlidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestVlidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email is required and must be a valid email address.");
            RuleFor(x => x.Code)
                .NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        }
    }
}
