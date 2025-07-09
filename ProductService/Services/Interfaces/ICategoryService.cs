namespace ProductService.Services.Interfaces;

using ProductService.Dto.OutDto;
using ProductService.Dto.InDto;

public interface ICategoryService
{
    Task<string> CreateCategory(string categoryName);
    Task<CategoryResponseDTO> GetCategoryById(string id);
    Task<List<CategoryResponseDTO>> GetAllCategories(int offset, int limit);
    Task<bool> UpdateCategory(CategoryUpdateDTO categoryUpdateDTO);
    Task<bool> DeleteCategory(string id);

    Task<bool> AddTypesToCategory(TypeCatAssociationDTO typeCatAssociatioDTO);
}
