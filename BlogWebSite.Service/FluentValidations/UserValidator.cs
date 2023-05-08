using BlogWebSite.Entity.Entities.Concrete;
using FluentValidation;

namespace BlogWebSite.Service.FluentValidations
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(_ => _.FirstName)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(20)
                .WithName("İsim");

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Soyisim");

            RuleFor(_ => _.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithName("Email Adresi");

        }
    }
}
