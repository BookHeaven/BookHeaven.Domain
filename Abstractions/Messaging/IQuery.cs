using Mediator;

namespace BookHeaven.Domain.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}