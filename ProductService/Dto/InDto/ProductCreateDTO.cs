namespace ProductService.Dto.InDto;

public record ProductCreateDTO
(
    string Name,
    string? IdType,
    string? Description,
    double Height,
    double Width,
    double Depth,
    double Price,
    ICollection<string>? Images
);
