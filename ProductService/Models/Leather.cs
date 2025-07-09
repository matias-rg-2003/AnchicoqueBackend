namespace ProductService.Models;

using ProductService.Models.Enums;

public class Leather
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Picture { get; set; }
    public required LeatherType Type { get; set; }

    //un cuero esta en muchas lineas
    public List<Line> Lines { get; set; } = [];
}
