using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day15Tests : DayTestBase
{
    public Day15Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day15.example.input.txt", 10092)]
    [InlineData("Day15.example.small.input.txt", 2028)]
    [InlineData("Day15.input.txt", 1441031)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day15 = new Day15(input, Logger);

        // Act
        var result = day15.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day15.example.input.txt", 1)]
    [InlineData("Day15.input.txt", 1)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day15 = new Day15(input, Logger);

        // Act
        var result = day15.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
