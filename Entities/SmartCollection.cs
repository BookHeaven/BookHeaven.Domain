using BookHeaven.Domain.Entities.Base;
using BookHeaven.Domain.Entities.Utilities;
using BookHeaven.Domain.Enums;

namespace BookHeaven.Domain.Entities;

public class SmartCollection : Collection
{
    public FilterSet<Guid> Authors { get; set; } = new();
    public FilterSet<Guid> Series { get; set; } = new();
    public FilterSet<Guid> Tags { get; set; } = new();
    public FilterSet<BookStatus> Statuses { get; set; } = new();
}