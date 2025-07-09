namespace ProductService.Services.Interfaces;

using ProductService.Dto.InDto;
using ProductService.Dto.OutDto;

public interface ILeatherService
{
    Task<LeatherResponseDTO> GetLeatherById(string id);
    Task<List<LeatherResponseDTO>> GetAllLeathers(int offset, int limit);
    Task<string> CreateLeather(LeatherCreateDTO leatherCreateDTO);
    Task<bool> UpdateLeather(LeatherUpdateDTO leatherUpdateDTO);
    Task<bool> DeleteLeather(string id);

}
