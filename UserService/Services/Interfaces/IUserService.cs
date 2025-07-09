namespace UserService.Services.Interfaces;

using UserService.Dto.InDto;
using UserService.Dto.OutDto;

public interface IUserService
{
    Task<UserResponseDTO> GetUserById(string id);
    Task<string> CreateUser(UserCreateDTO userCreateDTO);
    Task<bool> UpdateUser(string id, UserUpdateDTO userUpdateDTO);
    Task<bool> DeleteUser(string id);
}
