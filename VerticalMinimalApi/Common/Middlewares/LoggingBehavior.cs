using MediatR;
using Serilog;

namespace VerticalMinimalApi.Common.Middlewares;

public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest: IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log.Information("Starting Request {@RequestName}\n Body Content: {@request}", typeof(TRequest).Name, request);
        var result = await next();
        Log.Information("Completed Request {@RequestName} \n Return Content: {@response}", typeof(TRequest).Name, result);
        return result;
    }
}