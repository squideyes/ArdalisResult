using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;

namespace Ardalis.Result.ErrorsLoggingDemo;

public static class SerilogHelper
{
    public static void InitLogDotLogger(
        LogLevel minLogLevel = LogLevel.Information,
            Action<LoggerConfiguration> configure = null!)
    {
        Serilog.Debugging.SelfLog.Enable(
            output => Debug.WriteLine(output));

        Serilog.Debugging.SelfLog.Enable(Console.Error);

        var config = new LoggerConfiguration()
            .MinimumLevel.Is(minLogLevel.ToLogEventLevel())
            .OmitTypeField<BasicContext>()
            .Destructure.ByTransforming<DateTime>(v => v.ToString("MM/dd/yyyy HH:mm:ss.fff"))
            .Destructure.ByTransforming<Enum>(v => v.ToString())
            .Destructure.ByTransforming<Guid>(v => v.ToString("D"))
            .Destructure.ByTransforming<TimeSpan>(v => v.ToString(@"d\.hh\:mm\:ss\.fff"));

        config.Enrich.FromLogContext();

        config.WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate:
            "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}");

        configure?.Invoke(config);

        Log.Logger = config.CreateLogger();
    }

    public static LogEventLevel ToLogEventLevel(this LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            _ => throw new LoggingException("A valid \"logLevel\" must be supplied.")
        };
    }
}