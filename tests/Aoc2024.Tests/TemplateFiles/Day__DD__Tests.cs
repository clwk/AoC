using Aoc2024.TemplateFiles;

namespace Aoc2024.Tests.TemplateFiles;

public class Day__DD__Tests
{
    [Theory]
    [InlineData("Day__DD__.example.input.txt", 1)]
    [InlineData("Day__DD__.input.txt", 1)]
    public void Test_RunStar1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day__DD__ = new Day__DD__(input);

        // Act
        var result = day__DD__.RunStar1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day__DD__.example.input.txt", 1)]
    [InlineData("Day__DD__.input.txt", 1)]
    public void Test_RunStar2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day__DD__ = new Day__DD__(input);

        // Act
        var result = day__DD__.RunStar2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
