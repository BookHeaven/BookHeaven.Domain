namespace BookHeaven.Domain.Extensions;

public abstract class EntityExtensions<T> where T : class
{
    public T Clone()
    {
        return (T)MemberwiseClone();
    }
}