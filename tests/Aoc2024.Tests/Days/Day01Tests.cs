using Aoc2024.Days;

namespace Aoc2024.Tests.Days;

public class Day01Tests
{
    [Theory]
    [InlineData("Day01.example.input.txt", 11)]
    [InlineData("Day01.input.txt", 1603498)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day01 = new Day01(input);

        // Act
        var result = day01.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day01.example.input.txt", 31)]
    [InlineData("Day01.input.txt", 25574739)]
    public void Test_RunPart2(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day01 = new Day01(input);

        // Act
        var result = day01.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
