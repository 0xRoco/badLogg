using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace badLogg;

//TODO: Make this disposable?
public class LogManager
{
    private readonly bool _isInitialized;
    private bool _isConsoleCreated;
    
    private static LogManager? _instance;
    private readonly LogConfig _config;
    private readonly ILogger _fileLogger;
    private readonly ILogger _consoleLogger;
    
    private readonly ManualResetEvent _hasNewLogs = new(false);
    private readonly Queue<Action> _logQueue = new();
    public LogManager(LogConfig config)
    {
        _instance = this;
        _config = config;
        _fileLogger = new FileLogger(_config);
        _consoleLogger = new ConsoleLogger(_config);
        _isInitialized = true;

        ProcessQueue();
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
            AddToQueue(() => _fileLogger.Info(message, callerName, callerPath));      
        }
        
        if (_config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => _consoleLogger.Info(message, callerName, callerPath));
        }
        
    }
    
    public void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");
        
        if (_config.IsFileLoggingEnabled)
        {
            AddToQueue(() => _fileLogger.Error(message, callerName, callerPath));      

        } 
        if (_config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => _consoleLogger.Error(message, callerName, callerPath));
        }

    }
    
    public void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");
        if (_config.IsFileLoggingEnabled)
        {
            AddToQueue(() => _fileLogger.Warn(message, callerName, callerPath));      
        }
        
        if (_config.IsConsoleLoggingEnabled)
        { 
            AddToQueue(() => _consoleLogger.Warn(message, callerName, callerPath));
        }

    }
    
    public void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!_isInitialized) throw new Exception("LogManager is not initialized");
        if (_config.IsFileLoggingEnabled)
        {
            AddToQueue(() => _fileLogger.Debug(message, callerName, callerPath));
        }
        if (_config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => _consoleLogger.Debug(message, callerName, callerPath));
        }

    }
    
    private void AddToQueue(Action action)
    {
        lock (_logQueue)
        {
            _logQueue.Enqueue(action);
        }
        _hasNewLogs.Set();
    }
    
    //TODO: flush logs from queue on demand
    //BUG: Logs are not printed in order in console
    private void ProcessQueue()
    {
        Task.Run(() =>
        {
            while (true)
            {
                _hasNewLogs.WaitOne();
                _hasNewLogs.Reset();

                Queue<Action> queueCopy;
                lock (_logQueue)
                {
                    queueCopy = new Queue<Action>(_logQueue);
                    _logQueue.Clear();
                }

                foreach (var log in queueCopy)
                {
                    log();
                }
            }
        });
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