using System.Collections.ObjectModel;

namespace VerticalMinimalApi.Common.Error;

public class BaseError
{
    public string Title { get; private set; }
    public string Message { get; private set; }
    public string? Detail { get; private set; }
    
    private BaseError(string title, string message, string? detail)
    {
        Title = title;
        Message = message;
        Detail = detail;
    }

    public static BaseError New(string title = "Error", string message = "Unexpected error", string? detail = null)
    {
        return new BaseError(title, message, detail);
    }

    public ICollection<BaseError> ToList()
    {
        Console.WriteLine(this);
        return new Collection<BaseError> { this };
    }
}