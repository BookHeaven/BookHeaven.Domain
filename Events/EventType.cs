using BookHeaven.Domain.Abstractions.Events;

namespace BookHeaven.Domain.Events;

public record BookAdded(Guid BookId) : IEvent;
public record BookUpdated(Guid BookId) : IEvent;
public record BookDeleted(Guid BookId) : IEvent;