using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day05Tests : DayTestBase
{
    public Day05Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }
    [Theory]
    [InlineData("Day05.example.input.txt", 143)]
    [InlineData("Day05.input.txt", 5129)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day05 = new Day05(input, Logger);

        // Act
        var result = day05.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day05.example.input.txt", 123)]
    [InlineData("Day05.input.txt", 4077)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day05 = new Day05(input, Logger);

        // Act
        var result = day05.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
