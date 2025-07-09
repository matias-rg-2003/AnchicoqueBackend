namespace UserService.Services.Implementations;

using UserService.Database;
using UserService.Dto.InDto;
using UserService.Dto.OutDto;
using UserService.Services.Interfaces;
using UserService.Models.Enums;
using UserService.Models;
using UserService.Exceptions;
using Microsoft.EntityFrameworkCore;
using UserService.Validations;
using FluentValidation.Results;
using FluentValidation;

public class UserServiceImpl(UserServiceContext dbContext) : IUserService
{
    private readonly UserServiceContext _dbContext = dbContext;


    public async Task<UserResponseDTO> GetUserById(string id)
    {

        var user = await _dbContext.Users!
        .Where(u => u.Id == id && u.State != UserState.INACTIVE)
        .FirstOrDefaultAsync() ?? throw new EntityNotFoundException("User not found");

        return new UserResponseDTO
        (
            user.Id,
            user.Name,
            user.LastName,
            user.Email,
            user.CreatedAt,
            user.Role,
            user.State
        );
    }


    public async Task<string> CreateUser(UserCreateDTO userCreateDTO)
    {
        UserValidator val = new();
        ValidationResult result = val.Validate(userCreateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        if (await _dbContext.Users!.AnyAsync(u => u.Email == userCreateDTO.Email))
            throw new ValidationException("Email already used");

        /* 
            TODO hashear contraseña 
        */
        //Crear el usuario
        User NewUser = new()
        {
            Id = Guid.NewGuid().ToString(), // Id generado automaticamente
            Name = userCreateDTO.Name,
            LastName = userCreateDTO.LastName,
            PhoneNumber = userCreateDTO.PhoneNumber,
            Email = userCreateDTO.Email,
            Password = userCreateDTO.Password, // TODO agregar contraseña hasheada
            CreatedAt = DateTime.UtcNow,
            Role = UserRole.CUSTOMER,
            State = UserState.ACTIVE
        };

        _dbContext.Users!.Add(NewUser); //Agregar el usuario a la base de datos
        await _dbContext.SaveChangesAsync();

        return NewUser.Id;
    }

    public async Task<bool> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
    {
        UserUpdateValidator val = new();
        ValidationResult result = val.Validate(userUpdateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        var UserToUpdate = await _dbContext.Users!.FindAsync(id) ?? throw new EntityNotFoundException("Entity not found"); //revisar por que no tira excepcion

        if (UserToUpdate.State == UserState.INACTIVE) throw new InvalidOperationException("User is inactive, cannot update");

        //Se actualiza el usuario
        UserToUpdate.Name = userUpdateDTO.Name;
        UserToUpdate.LastName = userUpdateDTO.LastName;
        UserToUpdate.PhoneNumber = userUpdateDTO.PhoneNumber;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var userToDelete = await _dbContext.Users.FindAsync(id)
        ?? throw new EntityNotFoundException("Entity not found");

        if (userToDelete.State == UserState.INACTIVE)
            throw new InvalidOperationException("User is already inactive");

        userToDelete.State = UserState.INACTIVE;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    Task<UserResponseDTO> IUserService.GetUserById(string id)
    {
        throw new NotImplementedException();
    }
}
