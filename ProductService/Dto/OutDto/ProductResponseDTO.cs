namespace ProductService.Dto.OutDto;

using ProductService.Dto.BasicDto;

public record ProductResponseDTO
(
    string Id,
    string Name,
    BProductTypeDTO? Type,
    string? Description = null,
    double? Height = null,
    double? Width = null,
    double? Depth = null,
    double? Price = null,
    string? State = null,
    List<string>? Images = null,
    List<LineResponseDTO>? Lines = null
);
