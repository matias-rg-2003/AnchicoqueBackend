namespace ProductService.Dto.InDto;

public record LineLeatherAssociationDTO
(
    string IdLine,
    List<string> LeathersIds
);
