using System.Runtime.CompilerServices;

namespace Ardalis.Result.ErrorsLoggingDemo;

public record ResultErrorsDetails(
    string[] Errors);

public static partial class ILoggerExtenders
{
    public static void LogResultErrors(
        this ILogger logger,
        Result result,
        Dictionary<string, string> extraInfo = null!,
        Guid correlationId = default,
        [CallerMemberName] string calledBy = "")
    {
        if (!result.IsError())
            return;

        logger.ResultErrors(
            new ResultErrorsDetails([.. result.Errors]),
            new BasicContext(calledBy, result
                .GetCorrelationId(correlationId), extraInfo));
    }

    [LoggerMessage(
        EventId = EventIds.ResultErrors,
        EventName = nameof(ResultErrors),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = $"{nameof(ResultErrors)}={{@Details}};Context={{@Context}}")]
    private static partial void ResultErrors(
        this ILogger logger,
        ResultErrorsDetails details,
        BasicContext context);
}