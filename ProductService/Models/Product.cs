namespace ProductService.Models;

using ProductService.Models.Enums;

public class Product
{
    public required string Id { get; set; }
    public required string Name { get; set; }

    //Relacion 1 a N con ProductType
    public required string? IdType { get; set; } // El producto se puede crear inicialmente sin asignarle un tipo
    public ProductType? Type { get; set; } = null!;

    public string? Description { get; set; }
    public required double Height { get; set; }
    public required double Width { get; set; }
    public required double Depth { get; set; }
    public required double Price { get; set; }

    public required ProductState State { get; set; }

    public List<ProductImage> Images { get; set; } = []; // El producto se puede crear inicialmente sin ser agregado a una linea
    public List<Line> Lines { get; set; } = [];

}
