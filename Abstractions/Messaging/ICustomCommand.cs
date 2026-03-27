using BookHeaven.Domain.Shared;
using MediatR;

namespace BookHeaven.Domain.Abstractions.Messaging;

public interface ICustomCommand : IRequest<Result>, IBaseCommand
{
}

public interface ICustomCommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}