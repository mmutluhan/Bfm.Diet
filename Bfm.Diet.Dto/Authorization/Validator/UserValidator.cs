using FluentValidation;

namespace Bfm.Diet.Dto.Authorization.Validator
{
    public class UserValidator : AbstractValidator<KullaniciDto>
    {
        public UserValidator()
        {
            RuleFor(f => f.Adi).NotEmpty();
            RuleFor(f => f.Soyadi).NotEmpty();
            RuleFor(f => f.Email).NotEmpty();
        }
    }
}