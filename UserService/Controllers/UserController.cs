namespace UserService.Controllers;

using UserService.Dto.OutDto;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.Exceptions;
using UserService.Dto.InDto;
using UserService.Services.Interfaces;

[Route("[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("{id}")] //en un futuro se puede usar para mostrar el perfil del usuario
    public async Task<ActionResult<UserResponseDTO>> GetUser(string id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { sucess = false, message = ve.Message });
        }
        catch (EntityNotFoundException nfe)
        {
            return NotFound(new { success = false, message = nfe.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }


    [HttpPost("create")]
    public async Task<ActionResult> CreateUser(UserCreateDTO userCreateDTO)
    {
        try
        {
            var IdCreated = await _userService.CreateUser(userCreateDTO);
            return CreatedAtAction(nameof(GetUser), new { id = IdCreated }, new { success = true, message = "User created", IdCreated });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { sucess = false, message = ve.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
    {                                           //por ahora recibir id, pero en un futuro se obtendra el id del usuario autenticado
        try
        {
            await _userService.UpdateUser(id, userUpdateDTO);
            return Ok("User Updated");
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { sucess = false, message = ve.Message });
        }
        catch (EntityNotFoundException nfe)
        {
            return NotFound(new { success = false, message = nfe.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            await _userService.DeleteUser(id);
            return Ok("User Deleted");
        }
        catch (EntityNotFoundException nfe)
        {
            return NotFound(new { success = false, message = nfe.Message });
        }
        catch (InvalidOperationException ioe)
        {
            return BadRequest(new { success = false, message = ioe.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

}
