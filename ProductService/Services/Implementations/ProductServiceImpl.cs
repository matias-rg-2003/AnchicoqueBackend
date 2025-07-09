namespace ProductService.Services.Implementations;

using ProductService.Services.Interfaces;
using ProductService.Database;
using ProductService.Dto.OutDto;
using Microsoft.EntityFrameworkCore;
using ProductService.Exceptions;
using ProductService.Dto.BasicDto;
using ProductService.Models;
using ProductService.Dto.InDto;
using ProductService.Validations;
using FluentValidation.Results;
using FluentValidation;
using ProductService.Models.Enums;

public class ProductServiceImpl(ProductServiceContext dbContext) : IProductService
{
    private readonly ProductServiceContext _dbContext = dbContext;

    public async Task<ProductResponseDTO> GetProductById(string id)
    {
        var product = await _dbContext.Products!
        .Where(p => p.Id == id)
        .Include(p => p.Images)
        .Include(p => p.Lines)
            .ThenInclude(l => l.Leathers)
        .FirstOrDefaultAsync()
        ?? throw new EntityNotFoundException("Product not found");

        return new ProductResponseDTO
        (
            product.Id,
            product.Name,
            product.Type is not null
            ? new BProductTypeDTO(product.IdType, product.Type.TypeName) : null,
            product.Description,
            product.Height,
            product.Width,
            product.Depth,
            product.Price,
            product.State.ToString(),
            product.Images?.Select(img => img.Url).ToList() ?? [],
            [ .. product.Lines.Select(line => new LineResponseDTO(
                Id: line.Id,
                Name: line.Name,
                Status: line.Status.ToString()
            )) ]

        );

    }

    public async Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO)
    {
        ProductUpdateValidator prodVal = new();
        ValidationResult result = prodVal.Validate(productUpdateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        var ProdToUpdate = await _dbContext.Products!.FindAsync(productUpdateDTO.Id)
        ?? throw new EntityNotFoundException("Product not found");

        // Se actualiza el producto
        ProdToUpdate.Name = productUpdateDTO.Name;
        ProdToUpdate.Description = productUpdateDTO.Description;
        ProdToUpdate.Height = productUpdateDTO.Height;
        ProdToUpdate.Width = productUpdateDTO.Width;
        ProdToUpdate.Depth = productUpdateDTO.Depth;
        ProdToUpdate.Price = productUpdateDTO.Price;

        if (productUpdateDTO.NewImages != null && productUpdateDTO.NewImages.Count != 0)
        {
            var newImages = productUpdateDTO.NewImages
                .Where(url => !ProdToUpdate.Images.Any(img => img.Url == url)) // Evitar duplicados
                .Select(url => new ProductImage
                {
                    Url = url,
                    ProductId = ProdToUpdate.Id
                })
                .ToList();

            ProdToUpdate.Images.AddRange(newImages);
        }
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<string> CreateProduct(ProductCreateDTO productCreateDTO)
    {
        ProductValidator prodVal = new();
        ValidationResult result = prodVal.Validate(productCreateDTO);

        if (!result.IsValid) throw new ValidationException("Input not valid");

        if (await _dbContext.Products!.AnyAsync(p => p.Name.Equals(productCreateDTO.Name)))
            throw new ValidationException($"There is already a product with the name '{productCreateDTO.Name}'");

        string productId = Guid.NewGuid().ToString();

        //Crear el producto
        Product NewProduct = new()
        {
            Id = productId,
            Name = productCreateDTO.Name,
            IdType = productCreateDTO.IdType,
            Description = productCreateDTO.Description,
            Height = productCreateDTO.Height,
            Width = productCreateDTO.Width,
            Depth = productCreateDTO.Depth,
            Price = productCreateDTO.Price,
            State = ProductState.AVAILABLE,
            Images = productCreateDTO.Images?.Select(url => new ProductImage
            {
                Url = url,
                ProductId = productId // Asignar la clave foránea del producto
            }).ToList() ?? []

        };

        await _dbContext.Products.AddAsync(NewProduct);
        await _dbContext.SaveChangesAsync();
        return productId;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var productToDelete = await _dbContext.Products!.FindAsync(id)
        ?? throw new EntityNotFoundException("Product not found");

        if (productToDelete.State == ProductState.UNAVAILABLE)
            throw new InvalidOperationException("Product is already unavailable");

        productToDelete.State = ProductState.UNAVAILABLE;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
