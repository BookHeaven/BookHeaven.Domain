using BookHeaven.Domain.Entities.Base;

namespace BookHeaven.Domain.Entities;

public class SimpleCollection : Collection
{
    public List<Guid> BookIds { get; set; } = [];
}