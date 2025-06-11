using Serilog;
using Serilog.Sinks.OpenTelemetry;
using MEL = Microsoft.Extensions.Logging;

namespace Ardalis.Result.ErrorsLoggingDemo;

public class LoggerBuilder
{
    // Builds and then returns an initialized
    // Microsoft.Extension.Logging.ILogger
    public static MEL.ILogger Build(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false,false)
            .Build();

        var seqApiUri = new Uri(config["Serilog:SeqApiUri"]!);
        
        var seqApiKey = config["Serilog:SeqApiKey"]!;

        var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel),
            config["Serilog:LogLevel"]! ?? "Debug");

        SerilogHelper.InitLogDotLogger(logLevel, configure =>
        {
            // One of these need to be specified for each
            // logged "details" type
            configure.OmitTypeField<ResultErrorsDetails>();

            configure.WriteTo.OpenTelemetry(x =>
            {
                x.Endpoint = seqApiUri.AbsoluteUri;
                x.Protocol = OtlpProtocol.HttpProtobuf;
                x.Headers = new Dictionary<string, string>
                {
                    ["X-Seq-ApiKey"] = seqApiKey
                };
                x.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "ResultErrorsLoggingDemo"
                };
            });
        });

        using var loggerFactory = LoggerFactory.Create(
             builder => { builder.AddSerilog(); });

        return loggerFactory.CreateLogger<Program>();
    }
}