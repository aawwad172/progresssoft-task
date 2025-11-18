using System.Runtime.Serialization;

namespace ProgressSoft.Domain.Exceptions;

public class FileStreamEmptyException : Exception
{
    public FileStreamEmptyException(string? message) : base(message)
    {
    }

    public FileStreamEmptyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
