namespace ProductService.Validations;

using ProductService.Dto.InDto;
using FluentValidation;

public class LineValidator : AbstractValidator<LineCreateDTO>
{
    public LineValidator()
    {
        RuleFor(line => line.Name)
        .NotEmpty()
        .MaximumLength(20);


        RuleFor(line => line.Description)
        .MinimumLength(500);
    }
}
