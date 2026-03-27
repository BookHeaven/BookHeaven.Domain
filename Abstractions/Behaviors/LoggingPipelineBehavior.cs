using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Abstractions.Behaviors;

internal sealed class LoggingPipelineBehavior<TMessage, TResponse>(
    ILogger<LoggingPipelineBehavior<TMessage, TResponse>> logger) 
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{

    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = new();
        var name = typeof(TMessage).DeclaringType?.Name ?? typeof(TMessage).Name;
        logger.LogDebug("Handling {string} ({date})",name, DateTime.Now.ToString("g"));
        stopwatch.Start();
        var response = await next(message, cancellationToken);
        stopwatch.Stop();
        logger.LogDebug("Handled {string} in {long}ms", name, stopwatch.ElapsedMilliseconds);
        stopwatch.Reset();
        
        return response;
    }
}