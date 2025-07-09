namespace UserService.Validations;

using UserService.Dto.InDto;
using FluentValidation;

public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
{
    public UserUpdateValidator()
    {
        RuleFor(user => user.PhoneNumber).Length(10).NotEmpty()
        .WithMessage("Phone Number must be 10 characters long");
        RuleFor(user => user.Name).NotEmpty();
        RuleFor(user => user.LastName).NotEmpty();
    }
}
