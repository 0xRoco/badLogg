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
    private LogManager _logger = null!;
    
    [SetUp]
    public void Setup()
    {
        _logger = new LogManager(new LogConfig("badLogg",
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\badLogg\\UnitTest\\Logs",
            5,
            true,
            true));
        
        var logger = LogManager.GetLogger();
    }
    
    [Test]
    [Order(0)]
    public void IsLoggerCreated()
    {
        Assert.IsNotNull(_logger);
    }

    [Test]
    [Order(1)]
    public void CanCreateConsole()
    {
        Assert.IsNotNull(_logger);
        _logger.CreateConsole();
         _logger.Info("Created console");
    }
    
    [Test]
    [Order(2)]
    public void CanLogInfo()
    {
        Assert.IsNotNull(_logger);
        _logger.Info("CanLogInfo");
    }
    
    [Test]
    [Order(3)]
    public void CanLogError()
    {
        Assert.IsNotNull(_logger);
        _logger.Error("CanLogError");
    }
    
    [Test]
    [Order(4)]
    public void CanLogWarning()
    {
        Assert.IsNotNull(_logger);
        _logger.Warn("CanLogWarning");
    }
    
    [Test]
    [Order(5)]
    public void CanLogDebug()
    {
        Assert.IsNotNull(_logger);
        _logger.Debug("CanLogDebug");
    }
    
    [Test]
    [Order(6)]
    public void CanSpamLog()
    {
        Assert.IsNotNull(_logger);
        for (var i = 0; i < 100; i++)
        {
            _logger.Info($"CanSpamLog {i+1}");
        }
    }
    
    //This test requires manual review of the log file
    [Test]
    [Order(7)]
    public void CanLogFromMultipleTasks()
    {
        Assert.IsNotNull(_logger);
        
        Task.Run(() =>
        {
            for (var i = 0; i < 100; i++)
            {
                _logger.Info($"CanLogFromMultipleTasks #1 {i+1}");
            }        
        }).Wait();
        
         Task.Run(() =>
        {
            for (var i = 0; i < 100; i++)
            {
                _logger.Info($"CanLogFromMultipleTasks #2 {i+1}");

            }        
        }).Wait();
    }
    
    
    //This test requires manual review of the log file
    [Test]
    [Order(8)]
    public void CanLogParallel()
    {
        Assert.IsNotNull(_logger);
        Parallel.Invoke(() =>
        {
            for (var i = 0; i < 10; i++)
            {
                _logger.Info($"CanLogParallel #1 {i+1}");

            }
        }, () => 
        {
            for (var i = 0; i < 10; i++)
            {
                _logger.Info($"CanLogParallel #2 {i+1}");

            }
        });
    }

    [Test]
    [Order(99)]
    public void CanDestroyConsole()
    {
        Assert.IsNotNull(_logger);
        _logger.DestroyConsole();
        _logger.Info("Destroyed console");
    }
    
}