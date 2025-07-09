namespace UserService.Validations;

using FluentValidation;
using UserService.Dto.InDto;

public class UserValidator : AbstractValidator<UserCreateDTO>
{
    public UserValidator()
    {
        RuleFor(user => user.Email).EmailAddress().NotEmpty();
        RuleFor(user => user.Password).MinimumLength(8).NotEmpty().Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\-])[A-Za-z\d@$!%*?&\-]{8,}$")
        .WithMessage("Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character (@$!%*-?&).");
        RuleFor(user => user.PhoneNumber).Length(10).NotEmpty()
        .WithMessage("Phone Number must be 10 characters long");
        RuleFor(user => user.Name).NotEmpty();
        RuleFor(user => user.LastName).NotEmpty();
    }
}