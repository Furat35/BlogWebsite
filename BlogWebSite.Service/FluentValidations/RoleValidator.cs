using BlogWebSite.Entity.Entities.Concrete;
using FluentValidation;

namespace BlogWebSite.Service.FluentValidations
{
    public class RoleValidator : AbstractValidator<AppRole>
    {
        public RoleValidator()
        {
            RuleFor(_ => _.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .WithName("Rol");
        }
    }
}
