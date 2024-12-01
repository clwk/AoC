namespace Aoc2024.Days;

public class Day01
{
    private readonly List<int> _list1;
    private readonly List<int> _list2;

    public Day01(string[] input)
    {
        var parsedLines = ParseLines(input);

        _list1 = parsedLines.Select(tuple => tuple.Item1).OrderBy(x => x).ToList();
        _list2 = parsedLines.Select(tuple => tuple.Item2).OrderBy(x => x).ToList();
    }

    public int RunPart1() => _list1.Zip(_list2, (a, b) => Math.Abs(a - b)).Sum();

    public int RunPart2() => _list1.Sum(item1 => item1 * _list2.Count(x => x == item1));

    private static List<(int, int)> ParseLines(string[] input) =>
        input
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
            .ToList();
}
