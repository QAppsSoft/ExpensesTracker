using Api.Data.BaseObjects;

namespace Api.Data;

public class Category : IntEntityBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; }
}