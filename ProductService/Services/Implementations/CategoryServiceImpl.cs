/*


namespace ProductService.Services.Implementations;

using ProductService.Database;
using ProductService.Dto.BasicDto;
using ProductService.Dto.InDto;
using ProductService.Dto.OutDto;
using ProductService.Exceptions;
using ProductService.Models;
using ProductService.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CategoryService(AnchicoqueContext dbContext) : ICategoryService
{
    private readonly AnchicoqueContext _dbContext = dbContext;


    public async Task<string> CreateCategory(string categoryName)
    {
        //validar TODO crear filtro de validaciones

        if (await _dbContext.Categories!.AnyAsync(c => c.Name.Equals(categoryName)))
            throw new ValidationException($"There is already a category with the name '{categoryName}'");

        string categoryId = Guid.NewGuid().ToString();

        //crear la categoria
        Category newCategory = new()
        {
            Id = categoryId,
            Name = categoryName
            //se crea sin ningun tipo de producto asociado a la categoria
        };

        await _dbContext.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();

        return categoryId;
    }

    public async Task<CategoryResponseDTO> GetCategoryById(string id)
    {
        var category = await _dbContext.Categories!
        .Where(c => c.Id == id)
        .Include(c => c.ProductTypes)
        .FirstOrDefaultAsync()
        ?? throw new EntityNotFoundException("Category not found");

        return new CategoryResponseDTO
        (
            Id: category.Id,
            Name: category.Name,
            [.. category.ProductTypes.Select(pt => new BProductTypeDTO(
                Id: pt.Id,
                TypeName: pt.TypeName
            ))]
        );
    }

    public async Task<List<CategoryResponseDTO>> GetAllCategories(int offset, int limit)
    {
        var categories = await _dbContext.Categories!
        .ToListAsync() ?? throw new EntityNotFoundException("No categories found");

        return [.. categories.Select(cat => new CategoryResponseDTO(
            Id: cat.Id,
            Name: cat.Name,
            [.. cat.ProductTypes.Select(pt => new BProductTypeDTO(
                Id: pt.Id,
                TypeName: pt.TypeName
            ))]
        )).Skip(offset).Take(limit)];
    }

    public async Task<bool> UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
    {
        var categoryToUpdate = await _dbContext.Categories!.FindAsync(categoryUpdateDTO.Id)
        ?? throw new EntityNotFoundException("Category not found");

        categoryToUpdate.Name = categoryUpdateDTO.NewName;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategory(string id)
    {
        var categoryToDelete = await _dbContext.Categories!.FindAsync(id)
        ?? throw new EntityNotFoundException("Category not found");

        _dbContext.Categories.Remove(categoryToDelete);
        await _dbContext.SaveChangesAsync();

        return true;
    }

/*
    public async Task<bool> AddTypesToCategory(TypeCatAssociationDTO dto)
    {
        //validar -->Crear el filtro de validacion

        //Se busca la categoria y en la consulta se incluyen los tipos asociados a esta
        //categoria
        var categiry = await _dbContext.Categories
        .Include(c => c.ProductTypes)
        .FirstOrDefaultAsync(c => c.Id.Equals(dto.CategoryId))
        ?? throw new EntityNotFoundException("Category not found");

        //Obtener los tipos que se desean agregar
        var newTypes = await _dbContext.ProductTypes!
        .Where(type => dto.TypesIds.Contains(type.Id))
        .ToListAsync();

        //Verificar si todos los tipos existen
        if (newTypes.Count != dto.)
    }
}

*/
