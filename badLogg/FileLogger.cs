namespace badLogg;

internal class FileLogger : ILogger
{

    private string? CurrentLogFileName { get; set; }


    private static ReaderWriterLock Lock { get; } = new();
    private LogManager Logger { get; }
    private LogConfig Config { get;}
    public FileLogger(LogConfig config)
    {
        Config = config;
        Logger = LogManager.GetLogger();
    }

    public async void Info(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Info, message, callerName, callerPath);
        await WriteToFile(formattedMessage);
    }

    public async void Warn(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Warn, message, callerName, callerPath);
        await WriteToFile(formattedMessage);
    }
    

    public async void Error(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Error, message, callerName, callerPath);
        await WriteToFile(formattedMessage);
    }

    public async void Debug(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Debug, message, callerName, callerPath);
        await WriteToFile(formattedMessage);
    }
    
    
    private static string FormatMessage(LogLevel logLevel, string message, string callerName, string callerPath)
    {
        return $"{DateTime.Now:s}| {logLevel}|{LogHelper.GetClassName(callerPath)}.{callerName}: {message}";
    }

    private async Task WriteToFile(string message)
    {
        try
        {
            Lock.AcquireWriterLock(Timeout.Infinite);
            if (!Directory.Exists(Config.LogDirectory)) Directory.CreateDirectory(Config.LogDirectory);
            if (string.IsNullOrEmpty(CurrentLogFileName))
            {
                var files = Directory.GetFiles(Config.LogDirectory);
                if (files.Length >= Config.MaxLogs)
                    foreach (var s in files)
                        File.Delete(s);
                CurrentLogFileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}_{Config.AppName}.txt";
            }

            var filePath = Path.Combine(Config.LogDirectory, CurrentLogFileName);
            await using var file = new StreamWriter(filePath, true);
            await file.WriteLineAsync(message);
            file.Close();
        }
        finally
        {
            Lock.ReleaseWriterLock();
        }
    }
}