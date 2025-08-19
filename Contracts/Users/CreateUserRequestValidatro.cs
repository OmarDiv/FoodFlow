namespace FoodFlow.Contracts.Users
{
    public class CreateUserRequestValidatro : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidatro()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Roles)
            .NotNull() 
            .NotEmpty();


            RuleFor(x => x.Roles)
            .Must(x => x != null && x.Distinct().Count() == x.Count) 
            .WithMessage("Roles cannot be empty and must contain valid role names.")
            .When(x => x.Roles != null);
        }
    }
}
