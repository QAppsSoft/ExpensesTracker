namespace Api.Models.BaseObjects;

public interface IEntityBase<T>
{
    public T Id { get; set; }
}