using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using badLogg.Interfaces;
using badLogg.Util;

namespace badLogg.Core;

//TODO: Make this disposable?
public class LogManager
{
    private bool IsInitialized { get; set; }
    public bool IsConsoleCreated { get; private set; }
    
    private static LogManager? _instance;
    private LogConfig Config { get; } = null!;
    private ILogger FileLogger { get; } = null!;
    private ILogger ConsoleLogger { get; } = null!;

    private SafeLogger SafeLogger { get; } = null!;
    
    private readonly ManualResetEvent _hasNewLogs = new(false);
    private readonly ManualResetEvent _hasNewSafeLogs = new(false);
    private readonly Queue<Action> _logQueue = new();
    private readonly Queue<Action> _safeLogQueue = new();

    public LogManager(LogConfig config)
    {
        try
        {
            _instance = this;
            Config = config;
            SafeLogger = new SafeLogger();
            FileLogger = new FileLogger(Config);
            ConsoleLogger = new ConsoleLogger(Config);
            IsInitialized = true;
            Task.Run(ProcessSafeQueue);
            Task.Run(ProcessQueue);

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                config.IsFileLoggingEnabled = false;
                Warn($"Logging to file is only supported on Windows - Logging to file will be disabled");
            }
            
        }
        catch (Exception e)
        {
            SafeLog($"An error occurred while initializing the LogManager: {e.GetBaseException()}");
        }
    }

    public static LogManager GetLogger()
    {
        if (_instance == null)
        {
            throw new Exception("LogManager is not initialized");
        }

        return _instance;
    }
    
    public LogConfig GetConfig()
    {
        if (Config == null)
        {
            throw new Exception("LogConfig is not initialized");
        }
        return Config;
    }
    
    public void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!IsInitialized) throw new Exception("LogManager is not initialized");
        if (Config.IsFileLoggingEnabled)
        {
            AddToQueue(() => FileLogger.Info(message, callerName, callerPath));      
        }
        
        if (Config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => ConsoleLogger.Info(message, callerName, callerPath));
        }
        
    }
    
    public void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!IsInitialized) throw new Exception("LogManager is not initialized");
        
        if (Config.IsFileLoggingEnabled)
        {
            AddToQueue(() => FileLogger.Error(message, callerName, callerPath));      

        } 
        if (Config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => ConsoleLogger.Error(message, callerName, callerPath));
        }

    }
    
    public void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!IsInitialized) throw new Exception("LogManager is not initialized");
        if (Config.IsFileLoggingEnabled)
        {
            AddToQueue(() => FileLogger.Warn(message, callerName, callerPath));      
        }
        
        if (Config.IsConsoleLoggingEnabled)
        { 
            AddToQueue(() => ConsoleLogger.Warn(message, callerName, callerPath));
        }

    }
    
    public void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        if (!IsInitialized) throw new Exception("LogManager is not initialized");
        if (Config.IsFileLoggingEnabled)
        {
            AddToQueue(() => FileLogger.Debug(message, callerName, callerPath));
        }
        if (Config.IsConsoleLoggingEnabled)
        {
            AddToQueue(() => ConsoleLogger.Debug(message, callerName, callerPath));
        }
        

    }

    internal void SafeLog(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "")
    {
        async void Action() => await SafeLogger.Log($"{message}", callerName, callerPath);

        AddToQueue( Action, true);
    }

    private void AddToQueue(Action action, bool isSafeLog=false)
    {
        if (isSafeLog)
        {
            lock (_safeLogQueue)
            {
                _safeLogQueue.Enqueue(action);
            }
            _hasNewSafeLogs.Set();
        }
        else
        {
            lock (_logQueue)
            {
                _logQueue.Enqueue(action);
            }
            _hasNewLogs.Set();
        }
    }

    //TODO: flush logs from queue on demand
    private void ProcessQueue()
    {
        while (IsInitialized)
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
    }

    private void ProcessSafeQueue()
    {
        while (IsInitialized)
        {
            _hasNewSafeLogs.WaitOne();
            _hasNewSafeLogs.Reset();
                
            Queue<Action> queueCopy;
            lock (_safeLogQueue)
            {
                queueCopy = new Queue<Action>(_safeLogQueue);
                _safeLogQueue.Clear();
            }
                
            foreach (var log in queueCopy)
            {
                log();
            }
        }
    }
    
    [DllImport("kernel32.dll")]
    private static extern void AllocConsole();
    [DllImport("kernel32.dll")]
    private static extern void FreeConsole();

    public void CreateConsole()
    {
        try
        {
            if (!Config.IsConsoleLoggingEnabled || IsConsoleCreated ||
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Warn($"Console was not created because {nameof(Config.IsConsoleLoggingEnabled)} is false or {nameof(IsConsoleCreated)} is true or OS is not Windows");
                return;
            }

            AllocConsole();
            IsConsoleCreated = true;
        }
        catch (Exception e)
        {
            Error($"An error occurred while creating console: {e.GetBaseException()}");
        }
    }
    
    public void DestroyConsole()
    {
        try
        {
            if (!Config.IsConsoleLoggingEnabled || !IsConsoleCreated ||
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            FreeConsole();
            IsConsoleCreated = false;
            Info("Console destroyed");
        }
        catch (Exception e)
        {
            Error($"An error occurred while destroying console: {e.GetBaseException()}");
        }
    }
}