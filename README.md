# badLogg
badLogg is a simple logging library for .NET applications. it's kinda easy to set up and use. 

And as the name says, it's not that advanced, but I wanted to reinvent the wheel and see how it goes.

### Features
* Asynchronous logging
* Console and file logging
* Logging levels


### Setting up and getting started
```csharp
_logger = new LogManager(new LogConfig()
            .SetAppName("badLogg")
            .SetLogDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\badLogg\\Logs") // Set the log directory
            .SetMaxLogs(5) // Sets the maximum number of logs to keep
            .WithConsoleLogging() // Enables console logging
            .WithFileLogging()); // Enables file logging
            
_logger.Info("Hello, World!");
```
```csharp
var _logger = LogManager.GetLogger();
_logger.Info("Hello, World!");
```
### Logging Levels
```csharp
_logger.Info("Info");
_logger.Debug("Debug");
_logger.Warn("Warn");
_logger.Error("Error");
```
### displaying/destroying a console window
```csharp
_logger.CreateConsole();

_logger.DestroyConsole();
```
