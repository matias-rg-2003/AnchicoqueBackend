namespace ProductService.Services.Implementations;


using FluentValidation;
using FluentValidation.Results;
using ProductService.Database;
using ProductService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductService.Dto.OutDto;
using ProductService.Exceptions;
using ProductService.Dto.InDto;
using ProductService.Validations;
using ProductService.Models;
using ProductService.Models.Enums;

public class LeatherServiceImpl(ProductServiceContext dbContext) : ILeatherService
{
    private readonly ProductServiceContext _dbContext = dbContext;

    public async Task<LeatherResponseDTO> GetLeatherById(string id)
    {
        var leather = await _dbContext.Leathers!
        .Where(l => l.Id == id).FirstOrDefaultAsync()
        ?? throw new EntityNotFoundException("Leather not found");

        return new LeatherResponseDTO
        (
            Id: leather.Id,
            Name: leather.Name,
            PictureUrl: leather.Picture,
            Type: leather.Type.ToString()
        );
    }

    public async Task<List<LeatherResponseDTO>> GetAllLeathers(int offset, int limit)
    {
        var leathers = await _dbContext.Leathers!
        .ToListAsync() ?? throw new EntityNotFoundException("No leathers found");

        return [.. leathers.Select(leather => new LeatherResponseDTO(
            Id: leather.Id,
            Name: leather.Name,
            PictureUrl: leather.Picture,
            Type: leather.Type.ToString()
        )).Skip(offset).Take(limit)];
    }

    public async Task<string> CreateLeather(LeatherCreateDTO leatherCreateDTO)
    {
        LeatherValidator prodVal = new();
        ValidationResult result = prodVal.Validate(leatherCreateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        if (await _dbContext.Leathers!.AnyAsync(l => l.Name.Equals(leatherCreateDTO.Name)))
            throw new ValidationException($"tThere is already a leather with the name '{leatherCreateDTO.Name}'");

        string leatherId = Guid.NewGuid().ToString();

        Leather newLeather = new()
        {
            Id = leatherId,
            Name = leatherCreateDTO.Name,
            Picture = leatherCreateDTO.PictureUrl,
            Type = (LeatherType)leatherCreateDTO.Type
        };

        await _dbContext.Leathers.AddAsync(newLeather);
        await _dbContext.SaveChangesAsync();
        return leatherId;
    }

    public async Task<bool> UpdateLeather(LeatherUpdateDTO leatherUpdateDTO)
    {
        LeatherUpdateValidator prodVal = new();
        ValidationResult result = prodVal.Validate(leatherUpdateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        var leatherToUpdate = await _dbContext.Leathers!.FindAsync(leatherUpdateDTO.Id)
        ?? throw new EntityNotFoundException("Leather not found");

        //se actualiza el cuero
        leatherToUpdate.Name = leatherUpdateDTO.Name;
        leatherToUpdate.Picture = leatherUpdateDTO.PictureUrl;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLeather(string id)
    {
        var leatherToDelete = await _dbContext.Leathers!.FindAsync(id)
        ?? throw new EntityNotFoundException("Leather not found");

        _dbContext.Leathers.Remove(leatherToDelete); //POR AHORA SE ELIMINA
                                                     //TODO analizar si se requiere
                                                     //     borrado logico
        await _dbContext.SaveChangesAsync();
        return true;
    }

    Task<Dto.OutDto.LeatherResponseDTO> ILeatherService.GetLeatherById(string id)
    {
        throw new NotImplementedException();
    }

    Task<List<LeatherResponseDTO>> ILeatherService.GetAllLeathers(int offset, int limit)
    {
        throw new NotImplementedException();
    }

}
