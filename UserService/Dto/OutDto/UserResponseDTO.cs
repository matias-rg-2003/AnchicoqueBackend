using UserService.Models.Enums;

namespace UserService.Dto.OutDto;

public record UserResponseDTO
(
    string Id,
    string Name,
    string LastName,
    string Email,
    DateTime CreatedAt,
    UserRole Role,
    UserState State
);
