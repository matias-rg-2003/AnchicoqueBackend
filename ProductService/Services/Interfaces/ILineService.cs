namespace ProductService.Services.Interfaces;

using ProductService.Dto.OutDto;
using ProductService.Dto.InDto;

public interface ILineService
{
    Task<LineResponseDTO> GetLineById(string id);
    Task<List<LineResponseDTO>> GetAllLines(int offset, int limit);
    Task<string> CreateLine(LineCreateDTO lineCreateDTO);
    Task<bool> UpdateLine(LineUpdateDTO lineUpdateDTO);
    Task<bool> DeleteLine(string id);
    Task<bool> AddProductsToLine(LineProductAssociationDTO dto);
    Task<bool> AddLeathersToLine(LineLeatherAssociationDTO dto);
}
