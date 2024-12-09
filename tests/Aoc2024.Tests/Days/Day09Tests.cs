using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day09Tests : DayTestBase
{
    public Day09Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day09.example.input.txt", 1928L)]
    [InlineData("Day09.input.txt", 6395800119709L)]
    public void Test_RunPart1(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day09 = new Day09(input, Logger);

        // Act
        var result = day09.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day09.example.input.txt", 2858L)]
    [InlineData("Day09.input.txt", 6418529470362L)]
    public void Test_RunPart2(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day09 = new Day09(input, Logger);

        // Act
        var result = day09.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
