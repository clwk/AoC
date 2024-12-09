using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day09
{
    private readonly ILogger _logger;
    private readonly string[] _input;
    private readonly List<int> _blocks = [];

    // used blocks register: int id, (startPos, len)
    private readonly Dictionary<int, (int pos, int len)> _blockPosLenById = [];

    // free blocks register: pos, len
    private readonly Dictionary<int, int> _freeBlocksLenByPos = [];
    private int _maxId;
    private int _maxBlockNr;

    public Day09(string[] input, ILogger logger)
    {
        _input = input;
        _logger = logger;
    }

    private void ParseInput()
    {
        var id = 0;
        var input = _input[0];
        _maxBlockNr = input.Length - 1;

        for (var i = 0; i <= _maxBlockNr; i++)
        {
            var number = int.Parse(input[i].ToString());
            if (i % 2 == 0)
            {
                _blockPosLenById[id] = (_blocks.Count, number);

                for (var j = 0; j < number; j++)
                {
                    _blocks.Add(id);
                }
                id++;
            }
            else
            {
                _freeBlocksLenByPos[_blocks.Count] = number;

                for (var j = 0; j < number; j++)
                {
                    _blocks.Add(-1);
                }
            }

            _maxId = id - 1;
        }
    }

    public long RunPart1()
    {
        ParseInput();
        MoveBlocks();

        return GetChecksum();
    }

    private long GetChecksum()
    {
        var i = 0;
        var sum = 0L;
        while (_blocks[i] != -1)
        {
            sum += i * _blocks[i];
            i++;
        }

        return sum;
    }

    private void MoveBlocks()
    {
        var numberOfNonZero = _blocks.Count(s => s != -1);

        while (_blocks[numberOfNonZero] != -1)
        {
            {
                var lastEntry = _blocks
                    .Select((value, index) => new { value, index })
                    .LastOrDefault(x => x.value != -1);

                if (lastEntry != null)
                {
                    var firstEmptyIndex = _blocks.FindIndex(x => x == -1);

                    _blocks[firstEmptyIndex] = lastEntry.value;
                    _blocks[lastEntry.index] = -1;
                }
            }
        }
        _logger.LogInformation("hej {@blocks}", _blocks);
    }

    public long RunPart2()
    {
        ParseInput();

        var id = _maxId;

        while (id != -1)
        {
            _logger.LogInformation("Block id: {blockId}", id);
            var blockLen = _blockPosLenById[id].len;
            var moveToFreeBlock = _freeBlocksLenByPos.FirstOrDefault(s =>
                s.Key < _blockPosLenById[id].pos && s.Value >= blockLen
            );

            if (moveToFreeBlock.Value != 0)
            {
                _blockPosLenById[id] = (moveToFreeBlock.Key, blockLen);
                var newFreeBlock = new KeyValuePair<int, int>(
                    moveToFreeBlock.Key + blockLen,
                    moveToFreeBlock.Value - blockLen
                );
                _freeBlocksLenByPos.Remove(moveToFreeBlock.Key);

                if (newFreeBlock.Value > 0)
                {
                    _freeBlocksLenByPos.Add(newFreeBlock.Key, newFreeBlock.Value);
                }
            }

            id--;
        }

        return CalculateChecksumPart2();
    }

    private long CalculateChecksumPart2()
    {
        var sum = 0L;
        foreach (var block in _blockPosLenById)
        {
            _logger.LogInformation("Add checksum for {@block}", block);
            var startPos = block.Value.pos;
            for (var i = startPos; i < startPos + block.Value.len; i++)
            {
                sum += i * block.Key;
            }
        }

        return sum;
    }
}
