using Serilog;

namespace SikiSokoChatApp.Shared.CrossCuttingConcerns.Logging.Serilog;

/// <summary>
/// Base class for logger services.
/// </summary>
/// <remarks>
/// This abstract class provides basic logging methods such as Verbose, Fatal, Info, Warn, Debug, and Error.
/// Derived classes should initialize the `Logger` property to use these methods.
/// </remarks>
public abstract class LoggerServiceBase
{
    protected ILogger Logger { get; set; }

    public void Verbose(string message) => Logger.Verbose(message);

    public void Fatal(string message) => Logger.Fatal(message);

    public void Info(string message) => Logger.Information(message);

    public void Warn(string message) => Logger.Warning(message);

    public void Debug(string message) => Logger.Debug(message);

    public void Error(string message) => Logger.Error(message);
}