
namespace badLogg;

internal interface ILogger
{
    void Info(string message, string callerName = "", string callerPath = "");
    void Warn(string message, string callerName = "", string callerPath = "");
    void Error(string message,  string callerName = "", string callerPath = "");
    void Debug(string message,  string callerName = "", string callerPath = "");
}