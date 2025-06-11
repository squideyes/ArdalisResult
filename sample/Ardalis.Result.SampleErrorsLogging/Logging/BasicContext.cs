namespace Ardalis.Result.ErrorsLoggingDemo;

public record BasicContext(
    string CalledBy,
    string CorrelationId,
    Dictionary<string, string> ExtraInfo);