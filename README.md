# badLogg
badLogg is a simple logging library for .NET 6 applications. its kinda easy to setup and use. 

And as the name says, its not that advanced but i wanted to reinvent the wheel and see how it goes.

### Features
* Asynchronous logging (i think?)
* Console and file logging

## Setting up and getting started
```c#
_logger = new LogManager(new LogConfig("badLogg", //App name
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\badLogg\\Logs", // Logging Directory
            5, // Max log files
            true, // File logging enabled
            true)); // Console logging enabled
            
_logger.Info("Hello, World!");
```
```c#
var _logger = LogManager.GetLogger();
_logger.Info("Hello, World!");
```
## Logging Levels
```c#
_logger.Info("Info");
_logger.Debug("Debug");
_logger.Warn("Warn");
_logger.Error("Error");
```
### And if you want to display/destroy a console window
```c#
_logger.CreateConsole();

_logger.DestroyConsole();
```
