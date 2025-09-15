namespace BookHeaven.Domain.Entities.Utilities;

public class FilterSet<T>
{
    public List<T> Include { get; set; } = [];
    public List<T> Exclude { get; set; } = [];
}

