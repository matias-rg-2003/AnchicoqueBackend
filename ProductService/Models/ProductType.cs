namespace ProductService.Models;

public class ProductType
{
    public required string Id { get; set; }
    public required string TypeName { get; set; }

    //Relacion 1 a N con Categoria
    public required string? IdCategory { get; set; }
    public Category? Category { get; set; } = null!;

    //Relacion 1 a N con Product
    public List<Product> Products { get; set; } = [];
}
