using Xunit;

public class Day09Test
{
    [Theory]
    [InlineData("Day09.example.input.txt", 114)]
    [InlineData("Day09.input.txt", 1)]
    public void Test_RunA(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day09 = new Day09(input);

        // Act
        var result = day09.RunA();

        // Assert
        Assert.Equal(expectedResult, result); 
    }

    [Theory]
    [InlineData("Day09.example.input.txt", 1)]
    [InlineData("Day09.input.txt", 1)]
    public void Test_RunB(string inputFilename, int expectedResult)
    {
        // Arrange
        var input = File.ReadAllLines(inputFilename);
        var day09 = new Day09(input);

        // Act
        var result = day09.RunB();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
