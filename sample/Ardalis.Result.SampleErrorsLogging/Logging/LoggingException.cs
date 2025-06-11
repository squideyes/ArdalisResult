namespace Ardalis.Result.ErrorsLoggingDemo;

public class LoggingException(string message) 
    : Exception(message)
{
}