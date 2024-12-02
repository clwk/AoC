using Aoc2024.Days;

namespace Aoc2024.Tests.Days;

public class Day02Tests
{
    [Theory]
    [InlineData("Day02.example.input.txt", 2)]
    [InlineData("Day02.input.txt", 432)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day02 = new Day02(input);

        // Act
        var result = day02.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day02.example.input.txt", 4)]
    [InlineData("Day02.input.txt", 488)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day02 = new Day02(input);

        // Act
        var result = day02.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
