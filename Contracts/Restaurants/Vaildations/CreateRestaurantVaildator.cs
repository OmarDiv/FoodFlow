namespace FoodFlow.Contracts.Restaurants.Vaildations
{
    public class LoginRequestValidator : AbstractValidator<CreateRestaurantRequest>
    {
        public LoginRequestValidator()
        {

            RuleFor(x => x.Name)
                  .NotEmpty().WithMessage("{PropertyName} is required")
                  .MaximumLength(100).WithMessage("{PropertyName} must not be longer than 100 characters.");

            RuleFor(x => x.Description)
                  .MaximumLength(500).WithMessage("{PropertyName} must not be longer than 500 characters.")
                  .When(x => x.Description is not null);
            RuleFor(x => x.LogoUrl)
                 .MaximumLength(300).WithMessage("{PropertyName} must not be longer than 300 characters.")
                 .WithMessage("{PropertyName} is required");

            RuleFor(x => x.Address)
                  .NotEmpty().WithMessage("{PropertyName} is required")
                  .MaximumLength(200).WithMessage("{PropertyName} must not be longer than 200 characters.");

            RuleFor(x => x.PhoneNumber)
                  .NotEmpty()
                  .WithMessage("{PropertyName} is required")
                  .MaximumLength(20).WithMessage("{PropertyName} must not be longer than 20 characters.");
        }


    }
}
