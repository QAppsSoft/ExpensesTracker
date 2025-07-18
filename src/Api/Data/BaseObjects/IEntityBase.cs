namespace Api.Data.BaseObjects;

public interface IEntityBase<T>
{
    public T Id { get; set; }
}