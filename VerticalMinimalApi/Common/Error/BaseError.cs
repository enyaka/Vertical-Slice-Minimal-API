using System.Collections.ObjectModel;

namespace VerticalMinimalApi.Common.Error;

public class BaseError
{
    public string Title { get; private set; }
    public string Message { get; private set; }


    private BaseError(string title, string message)
    {
        Title = title;
        Message = message;
    }

    public static BaseError New(string title = "Error", string message = "Unexpected error")
    {
        return new BaseError(title, message);
    }

    public ICollection<BaseError> ToList()
    {
        Console.WriteLine(this);
        return new Collection<BaseError> { this };
    }
}