namespace ProductService.Validations;

using ProductService.Dto.InDto;
using FluentValidation;

public class LeatherUpdateValidator : AbstractValidator<LeatherUpdateDTO>
{
    public LeatherUpdateValidator()
    {
        RuleFor(leather => leather.Name)
        .MaximumLength(20);

        RuleFor(leather => leather.PictureUrl)
        .NotEmpty().WithMessage("You must add at least one picture");
    }
}
