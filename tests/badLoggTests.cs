using System;
using System.IO;
using System.Threading.Tasks;
using badLogg.Core;
using NUnit.Framework;

namespace tests;

// ReSharper disable once IdentifierTypo
// ReSharper disable once InconsistentNaming
public class badLoggTests
{
    private readonly LogManager _logger;

    public badLoggTests()
    {
        _logger = new LogManager(new LogConfig()
            .SetAppName("badLogg")
            .SetLogDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\badLogg\\UnitTest\\Logs")
            .SetMaxLogs(5)
            .WithConsoleLogging()
            .WithFileLogging());
    }

    [Test]
    [Order(0)]
    public void CanCreateLogger()
    {
        Assert.IsNotNull(_logger);
        _logger.Info("CanCreateLogger");
    }

    [Test]
    [Order(1)]
    public void CanCreateConsole()
    {
        _logger.CreateConsole();
        Assert.IsTrue(_logger.IsConsoleCreated);
        _logger.Info("CanCreateConsole");
    }

    [Test]
    [Order(2)]
    public void CanInfoLog()
    {
        _logger.Info($"This is a info log");
    }

    [Test]
    [Order(3)]
    public void CanWarningLog()
    {
        _logger.Warn($"This is a warning log");
    }

    [Test]
    [Order(4)]
    public void CanErrorLog()
    {
        _logger.Error($"This is a error log");
    }

    [Test]
    [Order(5)]
    public void CanDebugLog()
    {
        _logger.Debug($"This is a debug log");
    }
    
    [Test]
    [Order(6)]
    public void LogDirectoryCreated()
    {
        Assert.IsTrue(Directory.Exists(_logger.GetConfig().LogDirectory));
        _logger.Info("LogDirectoryCreated");
    }

    [Test]
    [Order(99)]
    public void CanDestroyConsole()
    {
        _logger.DestroyConsole();
        Assert.IsFalse(_logger.IsConsoleCreated);
        _logger.Info("CanDestroyConsole");
    }
}