using BlogWebSite.Entity.Models.DTOs.Users;
using FluentValidation;

namespace BlogWebSite.Service.FluentValidations
{
    public class UserProfileValidator : AbstractValidator<UserProfileDto>
    {
        public UserProfileValidator()
        {
            RuleFor(_ => _.FirstName)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(20)
               .WithName("İsim");
            RuleFor(_ => _.FirstName)
               .NotNull();

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Soyisim");
            RuleFor(_ => _.LastName)
                .NotNull();

            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress()
                .WithName("Email Adresi");
            RuleFor(_ => _.Email)
                .NotNull();

            RuleFor(_ => _.PhoneNumber)
              .NotEmpty()
              .NotNull()
              .MinimumLength(8)
              .MaximumLength(20)
              .WithName("Telefon");
            RuleFor(_ => _.PhoneNumber)
              .NotNull();

            RuleFor(_ => _.CurrentPassword)
              .NotEmpty()
              .WithName("Şifre");

        }
    }
}
