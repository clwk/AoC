using Aoc2024.Days;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day06Tests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger _logger;

    public Day06Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = new XUnitLoggerProvider(_testOutputHelper, appendScope: false).CreateLogger(
            nameof(Day06)
        );
    }

    [Theory]
    [InlineData("Day06.example.input.txt", 41)]
    [InlineData("Day06.input.txt", 4977)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day06 = new Day06(input, _logger);

        // Act
        var result = day06.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day06.example.input.txt", 6)]
    [InlineData("Day06.input.txt", 1729)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day06 = new Day06(input, _logger);

        // Act
        var result = day06.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
