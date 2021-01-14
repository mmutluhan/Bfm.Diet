using FluentValidation;

namespace Bfm.Diet.Dto.Authorization.Validator
{
    public class GroupValidator : AbstractValidator<GrupDto>
    {
        public GroupValidator()
        {
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.Description).NotEmpty();
        }
    }
}