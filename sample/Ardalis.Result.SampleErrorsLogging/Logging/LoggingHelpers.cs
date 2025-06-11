namespace Ardalis.Result.ErrorsLoggingDemo;

public static class LoggingHelpers
{
    public static string GetCorrelationId(
        this Result result, Guid correlationId)
    {
        if (correlationId != default)
            return correlationId.ToString("N");
        else if (string.IsNullOrEmpty(result.CorrelationId))
            return Guid.NewGuid().ToString("N");
        else
            return result.CorrelationId;
    }
}
