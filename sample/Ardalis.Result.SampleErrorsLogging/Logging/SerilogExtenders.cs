using Serilog;

namespace Ardalis.Result.ErrorsLoggingDemo;

public static class SerilogExtenders
{
    public static LoggerConfiguration OmitTypeField<T>(
        this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .Destructure.ByTransforming<T>(instance =>
        {
            var properties = instance!.GetType()
                .GetProperties().Where(p => p.Name != "$type")
                .ToDictionary(p => p.Name, p => p.GetValue(instance));

            return properties;
        });
    }
}