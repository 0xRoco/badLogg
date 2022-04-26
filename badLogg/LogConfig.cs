namespace badLogg;

public class LogConfig
{
    public LogConfig(string appName, string logDirectory, int maxLogs, bool isFileLoggingEnabled, bool isConsoleLoggingEnabled)
    {
        AppName = appName;
        LogDirectory = logDirectory;
        MaxLogs = maxLogs;
        IsFileLoggingEnabled = isFileLoggingEnabled;
        IsConsoleLoggingEnabled = isConsoleLoggingEnabled;
    }

    public string AppName { get; set; }
    public string LogDirectory { get; set; }
    public int MaxLogs { get; set; }
    public bool IsFileLoggingEnabled { get; set; }
    public bool IsConsoleLoggingEnabled { get; set; }
}