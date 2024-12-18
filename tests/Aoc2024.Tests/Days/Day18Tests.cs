using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day18Tests : DayTestBase
{
    public Day18Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day18.example.input.txt", 22, 6, 12)]
    [InlineData("Day18.input.txt", 348, 70, 1024)]
    public void Test_RunPart1(
        string inputFilename,
        int expectedResult,
        int mazeDimension,
        int nrOfBytes
    )
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day18 = new Day18(input, Logger, mazeDimension, nrOfBytes);

        // Act
        var result = day18.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day18.example.input.txt", "(6,1)", 6, 12)]
    [InlineData("Day18.input.txt", "(54,44)", 70, 1024)]
    public void Test_RunPart2(
        string inputFilename,
        string expectedResult,
        int mazeDimension,
        int nrOfBytes
    )
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day18 = new Day18(input, Logger, mazeDimension, nrOfBytes);

        // Act
        var result = day18.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
