using BookHeaven.Domain.Extensions;

namespace BookHeaven.Domain.Enums;

public enum CollectionType
{
    [StringValue("Simple")]
    Simple = 0,
    [StringValue("Smart")]
    Smart = 1
}