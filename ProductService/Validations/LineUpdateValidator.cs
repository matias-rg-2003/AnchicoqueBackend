using ProductService.Dto.InDto;
using FluentValidation;

namespace ProductService.Validations;

public class LineUpdateValidator : AbstractValidator<LineUpdateDTO>
{
    public LineUpdateValidator()
    {
        RuleFor(line => line.Name)
        .NotEmpty()
        .MaximumLength(20);


        RuleFor(line => line.Description)
        .MinimumLength(500);
    }
}
