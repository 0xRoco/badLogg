namespace badLogg;

internal class FileLogger : ILogger
{

    private string? CurrentLogFileName { get; set; }


    private readonly  ReaderWriterLockSlim _lock = new();
    private LogManager Logger { get; }
    private LogConfig Config { get;}
    public FileLogger(LogConfig config)
    {
        Config = config;
        Logger = LogManager.GetLogger();
    }

    public void Info(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Info, message, callerName, callerPath);
        WriteToFile(formattedMessage);
    }

    public  void Warn(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Warn, message, callerName, callerPath);
        WriteToFile(formattedMessage);
    }
    

    public  void Error(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Error, message, callerName, callerPath);
        WriteToFile(formattedMessage);
    }

    public  void Debug(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = FormatMessage(LogLevel.Debug, message, callerName, callerPath);
        WriteToFile(formattedMessage);
    }
    
    
    private static string FormatMessage(LogLevel logLevel, string message, string callerName, string callerPath)
    {
        return $"{DateTime.Now:s}| {logLevel}|{LogHelper.GetClassName(callerPath)}.{callerName}: {message}";
    }
    
    private void WriteToFile(string message)
    {
        _lock.EnterWriteLock();
        try
        {
            lock (_lock)
            {
                if (!Directory.Exists(Config.LogDirectory)) Directory.CreateDirectory(Config.LogDirectory);
                if (string.IsNullOrEmpty(CurrentLogFileName))
                {
                    var files = Directory.GetFiles(Config.LogDirectory);
                    if (files.Length >= Config.MaxLogs)
                        foreach (var s in files)
                            File.Delete(s);
                    CurrentLogFileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}_{Config.AppName}.txt";
                }

                File.AppendAllText($@"{Config.LogDirectory}\{CurrentLogFileName}", $"{message}\n");
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}