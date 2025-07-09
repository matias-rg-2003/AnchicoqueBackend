namespace ProductService.Services.Interfaces;

using ProductService.Dto.InDto;
using ProductService.Dto.OutDto;

public interface IProductService
{
    Task<ProductResponseDTO> GetProductById(string id);
    Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO);
    Task<string> CreateProduct(ProductCreateDTO productCreateDTO);
    Task<bool> DeleteProduct(string id);

}
