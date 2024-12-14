using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day14Tests : DayTestBase
{
    public Day14Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day14.example.input.txt", 12)]
    [InlineData("Day14.input.txt", 229069152)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day14 = new Day14(input, Logger);

        // Act
        var dimensions = inputFilename.Contains("example") ? (11, 7) : (101, 103);
        var result = day14.RunPart1(dimensions);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    //    [InlineData("Day14.example.input.txt", 1)]
    [InlineData("Day14.input.txt", 7383)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day14 = new Day14(input, Logger);

        // Act
        var result = day14.RunPart2((101, 103));

        // Assert
        result.Should().Be(expectedResult);
    }
}
