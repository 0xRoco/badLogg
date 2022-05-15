using badLogg.Util;

namespace badLogg.Interfaces;

internal interface ISafeLogger
{
    Task Log(string message, string callerName, string callerFilePath);
    
}