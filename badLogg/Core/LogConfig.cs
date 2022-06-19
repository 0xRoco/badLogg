namespace badLogg.Core;

public class LogConfig
{

    public string AppName { get; private set; } = "badLogg";
    public string LogDirectory { get; private set; } = "/Logs";
    public int MaxLogs { get; private set; } = 3;
    public bool IsFileLoggingEnabled { get; internal set; }
    public bool IsConsoleLoggingEnabled { get; internal set; }
    
    public LogConfig SetAppName(string appName)
    {
        AppName = appName;
        return this;
    }
    
    public LogConfig SetLogDirectory(string logDirectory)
    {
        LogDirectory = logDirectory;
        return this;
    }
    
    public LogConfig SetMaxLogs(int maxLogs)
    {
        MaxLogs = maxLogs;
        return this;
    }
    
    public LogConfig WithFileLogging()
    {
        IsFileLoggingEnabled = true;
        return this;
    }
    
    public LogConfig WithConsoleLogging()
    {
        IsConsoleLoggingEnabled = true;
        return this;
    }
}