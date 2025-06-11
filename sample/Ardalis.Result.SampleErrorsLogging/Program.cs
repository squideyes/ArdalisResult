using Ardalis.Result;
using Ardalis.Result.ErrorsLoggingDemo;
using Serilog;

// Build a configured logger
var logger = LoggerBuilder.Build(args);

// Log Result Errors, with optional ExtraInfo and an optional
// CorrelationId override, If a CorrelationId is not provided,
// it will be taken from Result.CorrelationId (if it exists)
// or created using Guid.NewGuid().
logger.LogResultErrors(
    Result.Error(new ErrorList(["Oops!", "Whoopsie!"])),
    new Dictionary<string, string>
    {
        { "Code", "ABC123" },
        { "Number", "987654321" }
    },
    Guid.NewGuid());

// Always close and flush before terminating your app
Log.CloseAndFlush();
