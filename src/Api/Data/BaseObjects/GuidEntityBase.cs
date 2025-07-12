namespace Api.Data.BaseObjects;

public class GuidEntityBase : IEntityBase<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}