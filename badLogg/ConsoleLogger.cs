namespace badLogg;

internal class ConsoleLogger : ILogger
{
    private LogConfig Config { get;}

    public ConsoleLogger(LogConfig config)
    {
        Config = config;
    }

    public async Task Info(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Info, message, callerName, callerPath);
        await PrintToConsole(LogLevel.Info, formattedMessage);
    }

    public async Task Warn(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Warn, message, callerName, callerPath);
        await PrintToConsole(LogLevel.Warn, formattedMessage);
    }

    public async Task Error(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Error, message, callerName, callerPath);
        await PrintToConsole(LogLevel.Error, formattedMessage);
    }

    public async Task Debug(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Debug, message, callerName, callerPath);
        await PrintToConsole(LogLevel.Debug, formattedMessage);
    }

    private static string FormatMessage(LogLevel logLevel, string message, string callerName = "", string callerPath = "")
    {
        return $"{DateTime.Now:s}| {logLevel}| {LogHelper.GetClassName(callerPath)}.{callerName}: {message}";
    }

    private static Task PrintToConsole(LogLevel logLevel ,string message)
    {
        Console.ForegroundColor = logLevel switch
        {
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warn => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Debug => ConsoleColor.DarkGray,
            _ => Console.ForegroundColor
        };
        Console.WriteLine($"{message}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
    
}