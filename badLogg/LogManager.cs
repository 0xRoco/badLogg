using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace badLogg;

public class LogManager
{
    private readonly bool _isInitialized;
    private bool _isConsoleCreated;
    
    private static LogManager? _instance;
    private readonly LogConfig _config;
    private readonly ILogger _fileLogger;
    private readonly ILogger _consoleLogger;
    public LogManager(LogConfig config)
    {
        _instance = this;
        _config = config;
        _fileLogger = new FileLogger(_config);
        _consoleLogger = new ConsoleLogger(_config);
        _isInitialized = true;
    }

    public static LogManager GetLogger()
    {
        if (_instance == null)
        {
            throw new Exception("LogManager is not initialized");
        }

        return _instance;
    }
    
    
    public void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");
        if (_config.IsFileLoggingEnabled)
        {
            _fileLogger.Info(message, callerName, callerPath);        
        }
        
        if (_config.IsConsoleLoggingEnabled)
        {
            _consoleLogger.Info(message, callerName, callerPath);
        }
        
    }
    
    public void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");
        
        if (_config.IsFileLoggingEnabled)
        {
            _fileLogger.Error(message, callerName, callerPath);

        } 
        if (_config.IsConsoleLoggingEnabled)
        {
            _consoleLogger.Error(message, callerName, callerPath);
        }

    }
    
    public void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");

        
        if (_config.IsFileLoggingEnabled)
        {
            _fileLogger.Warn(message, callerName, callerPath);
        }
        
        if (_config.IsConsoleLoggingEnabled)
        { 
            _consoleLogger.Warn(message, callerName, callerPath);
        }

    }
    
    public void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");

        
        if (_config.IsFileLoggingEnabled)
        {
            _fileLogger.Debug(message, callerName, callerPath);
        }
        if (_config.IsConsoleLoggingEnabled)
        {
            _consoleLogger.Debug(message, callerName, callerPath);
        }

    }
    
    [DllImport("kernel32.dll")]
    private static extern void AllocConsole();
    [DllImport("kernel32.dll")]
    private static extern void FreeConsole();

    public void CreateConsole()
    {
        if (!_config.IsConsoleLoggingEnabled || _isConsoleCreated || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }
        
        AllocConsole();
        _isConsoleCreated = true;
    }
    
    public void DestroyConsole()
    {
        if (!_config.IsConsoleLoggingEnabled || !_isConsoleCreated || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }
        
        FreeConsole();
        _isConsoleCreated = false;
    }
}