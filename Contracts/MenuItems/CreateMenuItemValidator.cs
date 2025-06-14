namespace FoodFlow.Contracts.MenuItems
{
    public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemRequest>
    {
        public CreateMenuItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100).WithMessage("{PropertyName} Must not be more than 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500).WithMessage("{PropertyName} Must not be more than 500 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
            //RuleFor(x => x.Category)
            //    .NotEmpty();
        }

    }

}
