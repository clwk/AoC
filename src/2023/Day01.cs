public class Day01(string[] input)
{
    private readonly Dictionary<string, int> DigitMapping =
        new()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

    public int RunA()
    {
        Console.WriteLine("Day 1");

        var firstLastDigits = input
            .Select(s => s.Where(char.IsDigit).ToList())
            .Select(digits => digits.First().ToString() + digits.Last().ToString())
            .Select(s => int.Parse(s))
            .ToList();

        System.Console.WriteLine(firstLastDigits.Sum());
        return firstLastDigits.Sum();
    }

    public int RunB()
    {
        // Write class name and method name to console dynamically
        Console.WriteLine(
            $"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod()?.Name}"
        );
        var inputTransformed = ReplaceStringDigits(input);

        var firstLastDigits = inputTransformed
            .Select(digits => digits.First().ToString() + digits.Last().ToString())
            .Select(int.Parse)
            .ToList();

        Console.WriteLine(firstLastDigits.Sum());
        return firstLastDigits.Sum();
    }

    // 54571 -
    // 54588 -

    internal List<string> ReplaceStringDigits(string[] input)
    {
        var output = new List<string>();

        foreach (var line in input)
        {
            var lineTransformed = new string(line);

            int positionsFound = 1;
            while (positionsFound > 0)
            {
                // find position of each string in digit mapping
                var positions = DigitMapping
                    .Select(kvp => (kvp.Key, kvp.Value, lineTransformed.IndexOf(kvp.Key)))
                    .ToList();
                var minPositionFound = positions.Where(p => p.Item3 > -1);

                positionsFound = minPositionFound.Count();
                if (positionsFound > 0)
                {
                    // var positionsMin = minPositionFound.MinBy(m => m.Item3);
                    var positionsMin = minPositionFound.MinBy(m => m.Item3);

                    lineTransformed = lineTransformed.Replace(
                        positionsMin.Key,
                        positionsMin.Value.ToString()
                    );
                }
            }

            // remove all non-digits from lineTransformed 
            var lineTransformedDigits = string.Concat(lineTransformed.Where(char.IsDigit));

            output.Add(lineTransformedDigits);
        }

        return output;
    }

    // internal List<string> ReplaceStringDigits(string[] input)
    // {
    //     return input
    //         .Select(line =>
    //         {
    //             while (true)
    //             {
    //                 var replacement = DigitMapping
    //                     .Select(
    //                         kvp => (Key: kvp.Key, Value: kvp.Value, Index: line.IndexOf(kvp.Key))
    //                     )
    //                     .Where(t => t.Index == 0)
    //                     .OrderByDescending(t => t.Key.Length)
    //                     .FirstOrDefault();

    //                 if (replacement == default)
    //                     break;

    //                 line = line.Remove(replacement.Index, replacement.Key.Length)
    //                     .Insert(replacement.Index, replacement.Value.ToString());
    //             }

    //             return string.Concat(line.Where(char.IsDigit));
    //         })
    //         .ToList();
    // }
}
