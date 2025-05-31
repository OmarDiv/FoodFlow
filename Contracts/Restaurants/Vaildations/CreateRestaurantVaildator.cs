using FluentValidation;
using FoodFlow.Contracts.Restaurants.Dtos;

namespace FoodFlow.Contracts.Restaurants.Vaildations
{
    public class CreateRestaurantVaildator : AbstractValidator<CreateRestaurantRequest>
    {
        public CreateRestaurantVaildator()
        {

            RuleFor(x => x.Name)
                  .NotEmpty().WithMessage("{PropertyName} name is required")
                  .MaximumLength(100).WithMessage("{PropertyName} name must not exceed 100 characters");

            RuleFor(x => x.Description)
                  .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters")
                  .When(x => x.Description != null);

            RuleFor(x => x.Address)
                  .NotEmpty().WithMessage("{PropertyName} is required")
                  .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters");

            RuleFor(x => x.PhoneNumber)
                  .NotEmpty()
                  .WithMessage("{PropertyName} number is required");
                  

            RuleFor(x => x.LogoUrl)
                  .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                  .WithMessage("Invalid image URL format")
                  .When(x => x.LogoUrl != null);
        }


    }
}
