using System.Net;
using FluentValidation;
using MediatR;
using Serilog;
using IResult = VerticalMinimalApi.Common.Base.IResult;

namespace VerticalMinimalApi.Common.Middlewares;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse: IResult, new()

{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null) return await next();
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) return await next();
        // foreach (var validationResultError in validationResult.Errors)
        //     Console.WriteLine(validationResultError.ErrorMessage);
        var result = new TResponse()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Errors = validationResult.Errors.Select(failure =>
                {
                    Log.Information("Invalid validation => {@message}", failure.ErrorMessage);
                    return new Error.Error()
                    {
                        Message = "Validation Error",
                        DisplayMessage = failure.ErrorMessage,
                        ErrorCode = failure.ErrorCode

                    };
                }
            )
        };
        
        return result;
    }
}