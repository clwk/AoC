using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day07
{
    private readonly ILogger _logger;
    private readonly List<(long Key, List<long> Values)> _numbers;
    private char[]? _operators;

    public Day07(string[] input, ILogger logger)
    {
        _logger = logger;
        _numbers = ParseInput(input);
    }

    public long RunPart1()
    {
        _operators = ['*', '+'];

        var sum = GetSumOfTrueCalibrations();

        return sum;
    }

    private long GetSumOfTrueCalibrations()
    {
        var sum = 0L;
        var count = 0;

        foreach (var (key, values) in _numbers)
        {
            var results = GenerateAndEvaluateExpressions(values, 0, values[0].ToString());

            if (results.Contains(key))
            {
                count++;
                sum += key;
                _logger.LogDebug("{calibration} was true", key);
            }
        }

        _logger.LogInformation(
            "Found {trueCalibrations} true calibrations of {calibrations}. Sum {sum}",
            count,
            _numbers.Count,
            sum
        );
        return sum;
    }

    private List<long> GenerateAndEvaluateExpressions(
        List<long> numbers,
        int index,
        string currentExpression
    )
    {
        var results = new List<long>();

        if (index == numbers.Count - 1)
        {
            results.Add(EvaluateExpression(currentExpression));
            return results;
        }

        foreach (var op in _operators ?? throw new ArgumentNullException())
        {
            var nextExpression = $"{currentExpression}{op}{numbers[index + 1]}";
            results.AddRange(GenerateAndEvaluateExpressions(numbers, index + 1, nextExpression));
        }

        return results;
    }

    private long EvaluateExpression(string expression)
    {
        var tokens = expression.Split(_operators);
        var operators = new List<char>();
        foreach (var ch in expression)
        {
            if (_operators != null && _operators.Contains(ch))
            {
                operators.Add(ch);
            }
        }

        var result = long.Parse(tokens[0]);
        for (var i = 1; i < tokens.Length; i++)
        {
            var value = long.Parse(tokens[i]);
            var op = operators[i - 1];
            result = op switch
            {
                '+' => result + value,
                '*' => result * value,
                '|' => long.Parse(string.Concat(result.ToString(), value.ToString())),
                _ => throw new ArgumentException($"Invalid operator: {op}"),
            };
        }

        return result;
    }

    private static List<(long Key, List<long> Values)> ParseInput(string[] input)
    {
        var result = new List<(long Key, List<long> Values)>();
        foreach (var line in input)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var key = long.Parse(parts[0].Trim());
                var values = parts[1].Trim().Split(' ').Select(long.Parse).ToList();
                result.Add((key, values));
            }
        }
        return result;
    }

    public long RunPart2()
    {
        _operators = ['*', '+', '|'];

        var sum = GetSumOfTrueCalibrations();

        return sum;
    }
}
