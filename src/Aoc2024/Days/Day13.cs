using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day13
{
    private readonly ILogger _logger;
    private readonly string[] _input;

    private List<(long x, long y)> _buttonA = [];
    private List<(long x, long y)> _buttonB = [];
    private List<(long x, long y)> _prize = [];

    public Day13(string[] input, ILogger logger)
    {
        _input = input;
        _logger = logger;
    }

    private void ParseInput(long adjust = 0)
    {
        _buttonA = [];
        _buttonB = [];
        _prize = [];

        for (var i = 0; i < _input.Length; i += 4)
        {
            var buttonA = ParseCoordinates(_input[i]);
            var buttonB = ParseCoordinates(_input[i + 1]);
            var prize = ParseCoordinates(_input[i + 2], adjust);

            _buttonA.Add(buttonA);
            _buttonB.Add(buttonB);
            _prize.Add(prize);
        }
    }

    private static (long x, long y) ParseCoordinates(string line, long adjust = 0)
    {
        var parts = line.Split(
            [' ', ',', ':', 'X', 'Y', '=', '+', 'A', 'B'],
            StringSplitOptions.RemoveEmptyEntries
        );

        return (long.Parse(parts[1]) + adjust, long.Parse(parts[2]) + adjust);
    }

    public long RunPart1()
    {
        ParseInput();

        var result = RunGameNumerics();
        return result.Sum();
    }

    private List<long> RunGame(int maxPresses = 100)
    {
        List<long> result = [];

        for (var i = 0; i < _prize.Count; i++)
        {
            var fewestTokens = long.MaxValue;

            for (var factorA = 1; factorA < maxPresses; factorA++)
            {
                for (var factorB = 1; factorB < maxPresses; factorB++)
                {
                    var xResult = factorA * _buttonA[i].x + factorB * _buttonB[i].x;
                    var yResult = factorA * _buttonA[i].y + factorB * _buttonB[i].y;
                    var xExpected = _prize[i].x;
                    var yExpected = _prize[i].y;

                    if (xResult == xExpected && yResult == yExpected)
                    {
                        var tokens = factorA * 3 + factorB * 1;
                        _logger.LogInformation(
                            "A {a}, B {b}, {tokens} tokens fulfil the result.",
                            factorA,
                            factorB,
                            tokens
                        );
                        if (tokens < fewestTokens)
                            fewestTokens = tokens;
                    }
                }
            }
            if (fewestTokens < int.MaxValue)
                result.Add(fewestTokens);
            else
            {
                _logger.LogDebug("No combination for {a}, {b} wins. ", _buttonA[i], _buttonB[i]);
            }
        }

        _logger.LogInformation("tokens: {@tokens}", result);
        return result;
    }

    private List<long> RunGameNumerics(long maxPresses = 100)
    {
        List<long> result = [];
        for (var i = 0; i < _prize.Count; i++)
        {
            var a = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { _buttonA[i].x, _buttonB[i].x },
                    { _buttonA[i].y, _buttonB[i].y },
                }
            );

            var b = Vector<double>.Build.Dense([_prize[i].x, _prize[i].y]);

            var solution = a.Solve(b);

            // Round the solutions to the nearest integers
            var factorA = (long)Math.Round(solution[0]);
            var factorB = (long)Math.Round(solution[1]);

            // Check if the rounded solutions are valid
            if (factorA > 0 && factorA < maxPresses && factorB > 0 && factorB < maxPresses)
            {
                var xResult = factorA * _buttonA[i].x + factorB * _buttonB[i].x;
                var yResult = factorA * _buttonA[i].y + factorB * _buttonB[i].y;

                // Verify that the results match the expected prize coordinates
                if (xResult == _prize[i].x && yResult == _prize[i].y)
                {
                    var tokens = factorA * 3 + factorB * 1;
                    _logger.LogInformation(
                        "A {a}, B {b}, {tokens} tokens fulfil the result.",
                        factorA,
                        factorB,
                        tokens
                    );

                    result.Add(tokens);
                }
                else
                {
                    _logger.LogDebug(
                        "No valid combination for {a}, {b} wins after double checking.",
                        _buttonA[i],
                        _buttonB[i]
                    );
                }
            }
            else
            {
                _logger.LogDebug(
                    "No valid combination for {a}, {b} wins.",
                    _buttonA[i],
                    _buttonB[i]
                );
            }
        }

        return result;
    }

    public long RunPart2()
    {
        const long adjust = 10000000000000L;
        ParseInput(adjust);

        var result = RunGameNumerics(1000000000000L);
        return result.Sum();
    }
}
