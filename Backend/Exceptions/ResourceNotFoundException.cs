namespace Backend.Exceptions;

public class ResourceNotFoundException(string message) : Exception(message);