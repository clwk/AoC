using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day13Tests : DayTestBase
{
    public Day13Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day13.example.input.txt", 480L)]
    [InlineData("Day13.input.txt", 36870L)]
    public void Test_RunPart1(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day13 = new Day13(input, Logger);

        // Act
        var result = day13.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day13.example.input.txt", 875318608908L)]
    [InlineData("Day13.input.txt", 78101482023732L)]
    public void Test_RunPart2(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day13 = new Day13(input, Logger);

        // Act
        var result = day13.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
