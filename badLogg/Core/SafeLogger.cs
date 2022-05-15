using badLogg.Helpers;
using badLogg.Interfaces;

namespace badLogg.Core;

internal class SafeLogger : ISafeLogger
{
    private static ReaderWriterLock Lock { get; } = new();
    private string CurrentLogFileName { get; set; } = "";
    
    public async Task Log(string message, string callerName = "", string callerPath = "")
    {
        var formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}| SafeLog| {LogHelper.GetClassName(callerPath)}.{callerName}: {message}";
        await WriteLog(formattedMessage); 
        await WriteConsole(formattedMessage);
        
    }

    private async Task WriteLog(string message)
    {
        try
        {
            Lock.AcquireWriterLock(Timeout.Infinite);
            CurrentLogFileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}_badLogg-SafeLog.txt";
            var filePath = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", CurrentLogFileName);
            await using var file = new StreamWriter(filePath, true);
            await file.WriteLineAsync(message);
            file.Close();
        }
        finally
        {
            Lock.ReleaseWriterLock();
        }
    }
    
    private static Task WriteConsole(string message)
    {
        try
        {
            Lock.AcquireWriterLock(Timeout.Infinite);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            return Task.CompletedTask;
        }
        finally
        {
            Lock.ReleaseWriterLock();
        }
    }
}