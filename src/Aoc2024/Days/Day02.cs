namespace Aoc2024.Days;

public class Day02
{
    private readonly List<List<int>> _allReports;

    public Day02(string[] input)
    {
        _allReports = input.Select(ParseReport).ToList();
    }

    public int RunPart1()
    {
        return _allReports.Count(IsReportSafe);
    }

    public int RunPart2()
    {
        return _allReports.Count(IsReportSafeExtended);
    }

    private static List<int> ParseReport(string line)
    {
        return line.Split(' ').Select(int.Parse).ToList();
    }

    private static bool IsReportSafe(List<int> report)
    {
        return IsReportSafeAscending(report) || IsReportSafeDescending(report);
    }

    private static bool IsReportSafeAscending(List<int> report)
    {
        return report
            .Zip(report.Skip(1), (current, next) => (current, next))
            .All(pair => pair.next - pair.current <= 3 && pair.next > pair.current);
    }

    private static bool IsReportSafeDescending(List<int> report)
    {
        return report
            .Zip(report.Skip(1), (current, next) => (current, next))
            .All(pair => pair.current - pair.next <= 3 && pair.next < pair.current);
    }

    private static bool IsReportSafeExtended(List<int> report)
    {
        return report
            .Select((_, index) => report.Where((_, i) => i != index).ToList())
            .Any(IsReportSafe);
    }
}
