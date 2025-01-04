using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day10Tests : DayTestBase
{
    public Day10Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day10.small1.input.txt", 4)]
    [InlineData("Day10.example.input.txt", 36)]
    [InlineData("Day10.input.txt", 744)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day10 = new Day10(input, Logger);

        // Act
        var result = day10.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day10.example.input.txt", 81)]
    [InlineData("Day10.input.txt", 1651)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day10 = new Day10(input, Logger);

        // Act
        var result = day10.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
