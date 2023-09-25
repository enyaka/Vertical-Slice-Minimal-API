namespace VerticalMinimalApi.Common.Error;

public class Error
{
    public string? ErrorCode { get; init; }
    public required string Message { get; init; }
    public string? DisplayMessage { get; init; }

    public static Error FromException(Exception exception, string? displayMessage = null)
    {
        return new Error
        {
            Message = exception.Message,
            ErrorCode = exception.HResult.ToString(),
            DisplayMessage = string.IsNullOrWhiteSpace(displayMessage) ? "An unknown error has occurred, if the problem persists please contact support" : displayMessage
        };
    }
}