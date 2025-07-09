namespace ProductService.Validations;

using ProductService.Dto.InDto;
using FluentValidation;
using ProductService.Models.Enums;

public class LeatherValidator : AbstractValidator<LeatherCreateDTO>
{
    public LeatherValidator()
    {
        RuleFor(leather => leather.Name)
        .NotEmpty()
        .MaximumLength(20);

        RuleFor(leather => leather.PictureUrl)
        .NotEmpty().WithMessage("You must add at least one picture");

        RuleFor(leather => leather.Type)
            .Must(value => Enum.IsDefined(typeof(LeatherType), value))
            .WithMessage("Leather must have a valid type");
    }
}
