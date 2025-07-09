namespace ProductService.Dto.OutDto;

using ProductService.Dto.BasicDto;

public record CategoryResponseDTO
(
    string Id,
    string Name,
    List<BProductTypeDTO>? ProductTypes = null
);
