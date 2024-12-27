using Aoc2024.Days;
using Xunit.Abstractions;

namespace Aoc2024.Tests.Days;

public class Day23Tests : DayTestBase
{
    public Day23Tests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Theory]
    [InlineData("Day23.example.input.txt", 7)]
    [InlineData("Day23.input.txt", 1151)]
    public void Test_RunPart1(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day23 = new Day23(input, Logger);

        // Act
        var result = day23.RunPart1();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Day23.example.input.txt", "co,de,ka,ta")]
    [InlineData("Day23.input.txt", "ar,cd,hl,iw,jm,ku,qo,rz,vo,xe,xm,xv,ys")]
    public void Test_RunPart2(string inputFilename, string expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day23 = new Day23(input, Logger);

        // Act
        var result = day23.RunPart2();

        // Assert
        result.Should().Be(expectedResult);
    }
}
