namespace Core.Exceptions;

public class InvalidCredentialsException(string message) : BusinessException(message);
