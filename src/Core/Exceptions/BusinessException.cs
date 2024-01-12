namespace Core.Exceptions;

public abstract class BusinessException : Exception
{
    public List<string> Messages { get; }

    protected BusinessException(string message) : base(message)
    {
        Messages = [message];
    }
    
    protected BusinessException(List<string> messages) 
    {
        Messages = messages;
    }
}
