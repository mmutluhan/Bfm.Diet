using FluentValidation;

namespace Bfm.Diet.Dto.Authorization.Validator
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(f => f.Kullanici).NotEmpty().EmailAddress();
            RuleFor(f => f.Parola).NotEmpty();
        }
    }
}