using Aoc2024.TemplateFiles;
using Xunit.Abstractions;

namespace Aoc2024.Tests.TemplateFiles;

public class Day__DD__Tests : DayTestBase
{
    public Day__DD__Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day__DD__.example.input.txt", 1)]
    [InlineData("Day__DD__.input.txt", 1)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day__DD__ = new Day__DD__(input, Logger);

        // Act
        var result = day__DD__.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day__DD__.example.input.txt", 1)]
    [InlineData("Day__DD__.input.txt", 1)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day__DD__ = new Day__DD__(input, Logger);

        // Act
        var result = day__DD__.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
