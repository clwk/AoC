using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day07Tests : DayTestBase
{
    public Day07Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day07.example.input.txt", 3749)]
    [InlineData("Day07.input.txt", 3351424677624)]
    public void Test_RunPart1(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day07 = new Day07(input, Logger);

        // Act
        var result = day07.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day07.example.input.txt", 11387)]
    [InlineData("Day07.input.txt", 204976636995111)]
    public void Test_RunPart2(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day07 = new Day07(input, Logger);

        // Act
        var result = day07.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
