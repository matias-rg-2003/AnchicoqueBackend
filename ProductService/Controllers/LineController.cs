namespace ProductService.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductService.Services.Interfaces;
using ProductService.Dto.OutDto;
using ProductService.Exceptions;
using ProductService.Dto.InDto;
using FluentValidation;

[Route("[controller]")]
[ApiController]
public class LineController(ILineService lineService) : ControllerBase
{
    private readonly ILineService _lineService = lineService;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<LineResponseDTO>> GetLine(string id)
    {
        try
        {
            var line = await _lineService.GetLineById(id);
            return Ok(line);
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
    public async Task<IActionResult> GetAllLines(int offset, int limit)
    {
        try
        {
            var lines = await _lineService.GetAllLines(offset, limit);
            return Ok(lines);
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
    public async Task<ActionResult> CreateLine(LineCreateDTO lineCreateDTO)
    {
        try
        {
            string IdCreated = await _lineService.CreateLine(lineCreateDTO);
            return CreatedAtAction(nameof(GetLine), new { id = IdCreated }, new { success = true, message = "Line created", IdCreated });
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
    public async Task<ActionResult> UpdateLine(LineUpdateDTO lineUpdateDTO)
    {
        try
        {
            await _lineService.UpdateLine(lineUpdateDTO);
            return Ok("Product updated");
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteLine(string id)
    {
        try
        {
            await _lineService.DeleteLine(id);
            return Ok("Product deleted");
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

    [HttpPatch("products")]
    public async Task<ActionResult> AddProductsToLine(LineProductAssociationDTO lineProductAssociationDTO)
    {
        try
        {
            await _lineService.AddProductsToLine(lineProductAssociationDTO);
            return Ok("Products added successfully");
        }
        catch (EntityNotFoundException nfe)
        {
            return NotFound(new { success = false, message = nfe.Message });
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

    [HttpPatch("leathers")]
    public async Task<ActionResult> AddLeathersToLine(LineLeatherAssociationDTO lineLeatherAssociationDTO)
    {
        try
        {
            await _lineService.AddLeathersToLine(lineLeatherAssociationDTO);
            return Ok("Leathers added successfully");
        }
        catch (EntityNotFoundException nfe)
        {
            return NotFound(new { success = false, message = nfe.Message });
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
}
