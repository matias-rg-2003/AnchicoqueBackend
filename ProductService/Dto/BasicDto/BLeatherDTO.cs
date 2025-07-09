namespace  ProductService.Dto.BasicDto;

using ProductService.Models.Enums;

public record BLeatherDTO
(
    string Id,
    string Name,
    LeatherType Type
);
