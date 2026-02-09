namespace smakchet.application.Exceptions;

public class DuplicateException(string message, string errorCode = "ResourceNotFound") : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}