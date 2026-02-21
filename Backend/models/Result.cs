namespace ADPHack.Backend.Models;

public class Result<T>
{
    public T? Payload { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; }

    public Result(T? payload, string? errorMessage, int statusCode)
    {
        Payload = payload;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }

    public Result() { }
}
