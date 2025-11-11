namespace ProgressSoft.Domain.Exceptions;

public class NotActiveUserException : Exception
{
    public NotActiveUserException(string? message) : base(message)
    {
    }

    public NotActiveUserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
