using System.Net;

namespace Domain.Responses;

public class Response<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; } = new List<string>();

    public Response(T data)
    {
        StatusCode = 200;
        Data = data;
    }

    public Response(T data,List<string> errors,HttpStatusCode statusCode)
    {
        Data = data;
        Errors = errors;
        StatusCode = (int)HttpStatusCode.OK;
    }

    public Response(HttpStatusCode statusCode,List<string> errors)
    {
        StatusCode = (int)statusCode;
        Errors = errors;
    }

    public Response(HttpStatusCode statusCode,string errors)
    {
        StatusCode = (int)statusCode;
        Errors.Add(errors);
    }
}
