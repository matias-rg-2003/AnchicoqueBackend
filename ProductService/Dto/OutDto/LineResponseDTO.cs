namespace ProductService.Dto.OutDto;

public record LineResponseDTO
(
    string Id,
    string Name,
    string Status,
    int? ProductCount = 0,
    string? Description = null,
    List<ProductResponseDTO>? Products = null,
    List<LeatherResponseDTO>? Leathers = null
);
