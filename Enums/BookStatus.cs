using BookHeaven.Domain.Extensions;
using BookHeaven.Domain.Localization;

namespace BookHeaven.Domain.Enums;

public enum BookStatus
{
    [StringValue(nameof(Translations.ALL_M))]
    All,
    New,
    [StringValue(nameof(Translations.READING))]
    Reading,
    Finished
}