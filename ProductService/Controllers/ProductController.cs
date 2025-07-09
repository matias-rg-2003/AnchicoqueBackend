namespace ProductService.Controllers;

using ProductService.Dto.InDto;
using ProductService.Dto.OutDto;
using ProductService.Exceptions;
using ProductService.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<ProductResponseDTO>> GetProduct(string id)
    {
        try
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
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
    public async Task<ActionResult> CreateProduct(ProductCreateDTO productCreateDTO)
    {
        try
        {
            string idCreated = await _productService.CreateProduct(productCreateDTO);
            return CreatedAtAction(nameof(GetProduct), new { id = idCreated }, new { success = true, message = "Product created" });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { success = false, message = ve.Message });
        }
        catch (InvalidOperationException ioe)
        {
            return BadRequest(new { success = false, message = ioe.Message });
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

    [HttpPut("update")]
    public async Task<ActionResult> UpdateProduct(ProductUpdateDTO productUpdateDTO)
    {
        try
        {
            await _productService.UpdateProduct(productUpdateDTO);
            return Ok("Product updated");
        }
        catch (Exception e)
        {
            return BadRequest(new { success = false, message = e.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        try
        {
            await _productService.DeleteProduct(id);
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

}
