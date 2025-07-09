namespace ProductService.Dto.InDto;

public record LineProductAssociationDTO
(
    string IdLine,
    List<string> ProductsIds
);
