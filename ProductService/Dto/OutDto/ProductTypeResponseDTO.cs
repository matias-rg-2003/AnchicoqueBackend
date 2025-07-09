namespace ProductService.Dto.OutDto;

using ProductService.Dto.BasicDto;

public record ProductTypeResponseDTO
(
    string Id,
    string TypeName,
    BCategoryDTO Category,
    List<BProductDTO> Products
);
