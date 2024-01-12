namespace Core.Exceptions;

public class InvalidArgumentException : BusinessException
{
    public InvalidArgumentException(string message) : base(message) { }

    public InvalidArgumentException(List<string> messages) : base(messages) { }
}
