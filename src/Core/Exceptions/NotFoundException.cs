namespace Core.Exceptions;

public class NotFoundException(string message) : BusinessException(message);
