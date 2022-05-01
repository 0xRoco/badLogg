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
    }
    
    //TODO: Add tests
}