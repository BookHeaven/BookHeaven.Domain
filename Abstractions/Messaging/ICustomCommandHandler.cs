using BookHeaven.Domain.Shared;
using MediatR;

namespace BookHeaven.Domain.Abstractions.Messaging;

public interface ICustomCommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICustomCommand
{
	
}

public interface ICustomCommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICustomCommand<TResponse>
{
	
}