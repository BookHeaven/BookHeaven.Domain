using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Abstractions.Behaviors;

internal sealed class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = new();
        logger.LogInformation("Handling {string} ({date})",typeof(TRequest).Name, DateTime.Now.ToString("g"));
        stopwatch.Start();
        var response = await next(cancellationToken);
        stopwatch.Stop();
        logger.LogInformation("Handled {string} in {long}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
        stopwatch.Reset();
        
        return response;
    }
}