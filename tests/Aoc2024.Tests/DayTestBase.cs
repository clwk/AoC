using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Aoc2024.Tests;

public abstract class DayTestBase
{
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly ILogger Logger;

    protected DayTestBase(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddProvider(new XUnitLoggerProvider(testOutputHelper))
                .SetMinimumLevel(LogLevel.Information);
        });

        Logger = loggerFactory.CreateLogger("TestLogger");
    }
}
