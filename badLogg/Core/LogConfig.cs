namespace badLogg.Core;

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

    public string AppName { get; internal set; }
    public string LogDirectory { get; internal set; }
    public int MaxLogs { get; internal set; }
    public bool IsFileLoggingEnabled { get; internal set; }
    public bool IsConsoleLoggingEnabled { get; internal set; }
}