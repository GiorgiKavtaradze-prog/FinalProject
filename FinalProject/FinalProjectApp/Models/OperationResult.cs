namespace FinalProjectApp.Models;

public sealed record OperationResult(bool Success, string Message)
{
    public static OperationResult Ok(string message) => new(true, message);
    public static OperationResult Fail(string message) => new(false, message);
    
    public static OperationResult<T> Ok<T>(T value, string message) => new(true, value, message);
    public static OperationResult<T> Fail<T>(string message) => new(false, default!, message);
}

public sealed record OperationResult<T>(bool Success, T Value, string Message)
{
    public static implicit operator OperationResult(OperationResult<T> result) => 
        new(result.Success, result.Message);
}