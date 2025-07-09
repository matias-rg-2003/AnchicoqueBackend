namespace UserService.Dto.InDto;

public record UserCreateDTO
(
    string Name,
    string LastName,
    string PhoneNumber,
    string Email,
    string Password
);