namespace ProductService.Dto.InDto;

public record TypeCatAssociationDTO
(
    string CategoryId,
    List<string> TypesIds
);
