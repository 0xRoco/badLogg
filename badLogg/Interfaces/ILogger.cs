
namespace badLogg.Interfaces;

internal interface ILogger
{
    Task Info(string message, string callerName = "", string callerPath = "");
    Task Warn(string message, string callerName = "", string callerPath = "");
    Task Error(string message,  string callerName = "", string callerPath = "");
    Task Debug(string message,  string callerName = "", string callerPath = "");
}