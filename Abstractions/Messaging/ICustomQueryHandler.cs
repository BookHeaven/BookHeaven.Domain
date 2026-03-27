using BookHeaven.Domain.Shared;
using MediatR;

namespace BookHeaven.Domain.Abstractions.Messaging;

public interface ICustomQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : ICustomQuery<TResponse>
{
    
}