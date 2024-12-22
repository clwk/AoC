using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day22Tests : DayTestBase
{
    public Day22Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day22.example.input.txt", 37327623L)]
    [InlineData("Day22.input.txt", 12759339434L)]
    public void Test_RunPart1(string inputFilename, long expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day22 = new Day22(input, Logger);

        // Act
        var result = day22.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day22.extra.example.input.txt", 23)]
    [InlineData("Day22.input.txt", 1405)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day22 = new Day22(input, Logger);

        // Act
        var result = day22.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
