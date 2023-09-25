using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace VerticalMinimalApi.Common.Base;

public interface IResult
{
    IEnumerable<Error.Error> Errors { get; set; }
    HttpStatusCode StatusCode { get; set; }
    
    
}