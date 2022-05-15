namespace badLogg.Helpers;

internal static class LogHelper
{
    internal static string GetClassName(string callerPath)
    {
        var className = callerPath.Split('\\').Last();
        className = className.Split('.').First();
        return className;
    }
}