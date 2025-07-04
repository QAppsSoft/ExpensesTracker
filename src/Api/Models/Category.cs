using Api.Models.BaseObjects;

namespace Api.Models;

public class Category : IntEntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}