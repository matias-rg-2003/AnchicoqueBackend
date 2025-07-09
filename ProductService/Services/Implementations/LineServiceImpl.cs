namespace ProductService.Services.Implementations;

using ProductService.Database;
using ProductService.Services.Interfaces;
using ProductService.Models.Enums;
using Microsoft.EntityFrameworkCore;
using ProductService.Exceptions;
using ProductService.Dto.BasicDto;
using ProductService.Dto.OutDto;
using ProductService.Dto.InDto;
using ProductService.Validations;
using FluentValidation;
using FluentValidation.Results;
using ProductService.Models;

public class LineServiceImpl(ProductServiceContext dbContext) : ILineService
{
    private readonly ProductServiceContext _dbContext = dbContext;

    public async Task<LineResponseDTO> GetLineById(string id)
    {
        var line = await _dbContext.Lines!
        .Where(l => l.Id == id && l.Status != LineState.UNAVAILABLE)
        .Include(l => l.Products)
        .Include(l => l.Leathers)
        .FirstOrDefaultAsync() ?? throw new EntityNotFoundException("Line not found");

        return new LineResponseDTO
        (
            line.Id,
            line.Name,
            line.Status.ToString(),
            line.ProductCount,
            line.Description,
            [.. line.Products.Select(prod => new ProductResponseDTO(
                Id: prod.Id,
                Name: prod.Name,
                Type: prod.Type is not null
                ? new BProductTypeDTO(prod.IdType, prod.Type.TypeName) : null,
                State: prod.State.ToString()
            ))],
            [.. line.Leathers.Select(leather => new LeatherResponseDTO(
                Id: leather.Id,
                Name: leather.Name,
                PictureUrl: leather.Picture,
                Type: leather.Type.ToString()
            ))]
        );
    }


    public async Task<List<LineResponseDTO>> GetAllLines(int offset, int limit)
    {
        var lines = await _dbContext.Lines!
        .ToListAsync() ?? throw new EntityNotFoundException("No lines found");

        return [.. lines.Select(line => new LineResponseDTO(
            Id: line.Id,
            Name: line.Name,
            Status: line.Status.ToString(),
            ProductCount: line.ProductCount,
            Description: line.Description
        )).Skip(offset).Take(limit)];
    }


    public async Task<string> CreateLine(LineCreateDTO lineCreateDTO)
    {
        LineValidator prodVal = new();
        ValidationResult result = prodVal.Validate(lineCreateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        if (await _dbContext.Lines!.AnyAsync(l => l.Name.Equals(lineCreateDTO.Name)))
            throw new ValidationException($"There is already a line with the name '{lineCreateDTO.Name}'");

        string lineId = Guid.NewGuid().ToString();

        //crear la linea
        Line NewLine = new()
        {
            Id = lineId,
            Name = lineCreateDTO.Name,
            Status = LineState.AVAILABLE,
            ProductCount = 0,
            Description = lineCreateDTO.Description
        };

        await _dbContext.Lines.AddAsync(NewLine);
        await _dbContext.SaveChangesAsync();
        return lineId;
    }

    public async Task<bool> UpdateLine(LineUpdateDTO lineUpdateDTO)
    {
        LineUpdateValidator prodVal = new();
        ValidationResult result = prodVal.Validate(lineUpdateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        var LineToUpdate = await _dbContext.Lines!.FindAsync(lineUpdateDTO.Id)
        ?? throw new EntityNotFoundException("Line not found");

        //Se actualiza la linea
        LineToUpdate.Name = lineUpdateDTO.Name;
        LineToUpdate.Description = lineUpdateDTO.Description;
        //las imagenes nuevas tambien se gestionan de la misma manera que cuando se crea, los productos y los cueros
        //se asocian luego de haber creado la linea.

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLine(string id)
    {
        var lineToDelete = await _dbContext.Lines!.FindAsync(id)
        ?? throw new EntityNotFoundException("Line not found");

        if (lineToDelete.Status == LineState.AVAILABLE)
            throw new InvalidOperationException("Line is already unavailable");

        lineToDelete.Status = LineState.UNAVAILABLE;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddProductsToLine(LineProductAssociationDTO dto)
    {
        LineProductAssociationValidator prodVal = new();
        ValidationResult result = prodVal.Validate(dto);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        var line = await _dbContext.Lines!
        .Include(l => l.Products)
        .FirstOrDefaultAsync(l => l.Id == dto.IdLine && l.Status == LineState.AVAILABLE)
        ?? throw new EntityNotFoundException("Line not found");

        //validar que los productos que se quieren agregar, existan
        var products = await _dbContext.Products!
        .Where(p => dto.ProductsIds.Contains(p.Id))
        .ToListAsync();

        if (products.Count != dto.ProductsIds.Count)
            throw new ValidationException("Some products do not exist.");

        // Filtrar productos que ya están en la línea
        var existingProductIds = line.Products.Select(p => p.Id).ToHashSet();
        var newProducts = products.Where(p => !existingProductIds.Contains(p.Id)).ToList();

        if (newProducts.Count == 0)
            throw new ValidationException("All products are already assigned to this line.");

        // Agregar los nuevos productos a la línea
        line.Products.AddRange(newProducts);
        line.ProductCount += newProducts.Count;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddLeathersToLine(LineLeatherAssociationDTO dto)
    {
        // Validar el DTO con un servicio de validación
        LineLeatherAssociationValidator prodVal = new();
        ValidationResult result = prodVal.Validate(dto);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        // Buscar la línea disponible con sus cueros relacionados
        var line = await _dbContext.Lines!
            .Include(l => l.Leathers)
            .FirstOrDefaultAsync(l => l.Id == dto.IdLine && l.Status == LineState.AVAILABLE)
            ?? throw new EntityNotFoundException("Line not found");

        // Obtener los cueros que se desean agregar
        var newLeathers = await _dbContext.Leathers!
            .Where(leather => dto.LeathersIds.Contains(leather.Id))
            .ToListAsync();

        // Verificar si todos los cueros existen
        if (newLeathers.Count != dto.LeathersIds.Count)
            throw new EntityNotFoundException("One or more leathers not found");

        // Agregar los nuevos cueros a la línea
        line.Leathers.AddRange(newLeathers);

        // Guardar cambios en la base de datos
        await _dbContext.SaveChangesAsync();
        return true;
    }




}
