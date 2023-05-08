using BlogWebSite.Entity.Models.DTOs.Users;
using FluentValidation;

namespace BlogWebSite.Service.FluentValidations
{
    public class UserAddDtoValidator : AbstractValidator<UserAddDto>
    {
        public UserAddDtoValidator()
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

            RuleFor(_ => _.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithName("Email Adresi");

            RuleFor(_ => _.RoleId)
                .NotEmpty()
                .NotNull()
                .WithName("Rol");

            RuleFor(_ => _.Password)
               .NotEmpty()
               .NotNull()
               .WithName("Şifre");

            RuleFor(_ => _.Photo)
               .NotEmpty()
               .NotNull()
               .WithName("Görsel");
        }
    }
}
