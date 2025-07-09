namespace ProductService.Dto.InDto;

public record ProductUpdateDTO
(
    string Id, //No lo proporciona el usuario
    string Name,
    string? Description,
    double Height,
    double Width,
    double Depth,
    double Price,
    ICollection<string>? NewImages //Imagenes nuevas o que se quieren agregar
);
