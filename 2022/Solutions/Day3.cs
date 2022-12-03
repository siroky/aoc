﻿using FuncSharp;

namespace AOC.Solutions;

public class Day3 : ISolver
{
    public IEnumerable<int> Solve(IEnumerable<string> lines)
    {
        var compartmentGroups = lines.Select(l => l.Chunk(l.Length / 2));
        var rucksackGroups = lines.Chunk(3);

        yield return DuplicatePriority(compartmentGroups);
        yield return DuplicatePriority(rucksackGroups);
    }

    private int DuplicatePriority(IEnumerable<IEnumerable<IEnumerable<char>>> groups)
    {
        var duplicates = groups.Select(g => GetDuplicate(g));
        return duplicates.Sum(i => GetPriority(i));
    }

    private char GetDuplicate(IEnumerable<IEnumerable<char>> group)
    {
        var duplicates = group.Aggregate((i, j) => i.Intersect(j));
        return duplicates.First();
    }

    private int GetPriority(char item)
    {
        return Char.IsLower(item).Match(
            t => 1 + item - 'a',
            f => 27 + item - 'A'
        );
    }
}