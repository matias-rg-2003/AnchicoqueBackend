namespace ProductService.Controllers;

using ProductService.Dto.OutDto;
using ProductService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductService.Exceptions;
using ProductService.Dto.InDto;
using FluentValidation;

[Route("[controller]")]
[ApiController]
public class LeatherController(ILeatherService leatherService) : ControllerBase
{
    private readonly ILeatherService _leatherService = leatherService;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<LeatherResponseDTO>> GetLeather(string id)
    {
        try
        {
            var leather = await _leatherService.GetLeatherById(id);
            return Ok(leather);
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

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllLeathers(int offset, int limit)
    {
        try
        {
            var leathers = await _leatherService.GetAllLeathers(offset, limit);
            return Ok(leathers);
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
    public async Task<ActionResult> CreateLeather(LeatherCreateDTO leatherCreateDTO)
    {
        try
        {
            string IdCreated = await _leatherService.CreateLeather(leatherCreateDTO);
            return CreatedAtAction(nameof(GetLeather), new { id = IdCreated }, new { success = true, message = "Leather created", IdCreated });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { success = false, message = ve.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

    [HttpPut("update")]
    public async Task<ActionResult> UpdateLeather(LeatherUpdateDTO leatherUpdateDTO)
    {
        try
        {
            await _leatherService.UpdateLeather(leatherUpdateDTO);
            return Ok("Leather updated");
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

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteLeather(string id)
    {
        try
        {
            await _leatherService.DeleteLeather(id);
            return Ok("Leather deleted");
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

