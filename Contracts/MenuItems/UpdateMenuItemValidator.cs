namespace FoodFlow.Contracts.MenuItems
{
    public class UpdateMenuItemValidator : AbstractValidator<UpdateMenuItemRequest>
    {
        public UpdateMenuItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100).WithMessage("{PropertyName} must not be more than 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500).WithMessage("{PropertyName} must not be more than 500 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
