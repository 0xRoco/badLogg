using System;
using System.Threading;
using System.Threading.Tasks;
using badLogg;
using NUnit.Framework;

namespace tests;

// ReSharper disable once IdentifierTypo
// ReSharper disable once InconsistentNaming
public class badLaggTests
{
    private readonly LogManager _logger;

    public badLaggTests()
    {
        _logger = new LogManager(new LogConfig("badLogg",
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\badLogg\\UnitTest\\Logs",
            5,
            true,
            true));
    }

    [Test]
    [Order(0)]
    public void CanCreateLogger()
    {
        Assert.IsNotNull(_logger);
    }

    [Test]
    [Order(1)]
    public void CanCreateConsole()
    {
        _logger.CreateConsole();
        Assert.IsTrue(_logger.IsConsoleCreated);
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
    public void CanLogParallel()
    {
        Parallel.For(0, 10, i =>
        {
            _logger.Info($"This is a info log {i}");
            _logger.Warn($"This is a warning log {i}");
            _logger.Error($"This is a error log {i}");
            _logger.Debug($"This is a debug log {i}");
        });
    }

    [Test]
    [Order(99)]
    public void CanDestroyConsole()
    {
        _logger.DestroyConsole();
        Assert.IsFalse(_logger.IsConsoleCreated);
    }
}