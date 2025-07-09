namespace ProductService.Models;

public class ProductImage
{
    public int Id { get; set; }
    public required string Url { get; set; } // URL de la imagen en Cloudinary
    public required string ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string? PublicId { get; set; } // ID de Cloudinary para gestionar la imagen
}
