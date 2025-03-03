﻿namespace AOC.Solutions;

public class Day14 : ISolver
{
    public IEnumerable<string> Solve(IEnumerable<string> lines)
    {
        var rocks = lines.Select(l => ParseRocks(l).ToList()).ToList();
        var origin = new Vector(500, 0);
        var grid1 = SimulateWaterfall(rocks, origin, floor: false);
        var grid2 = SimulateWaterfall(rocks, origin, floor: true);

        yield return grid1.Values.Where(c => c == CellType.Sand).Count().ToString();
        yield return grid2.Values.Where(c => c == CellType.Sand).Count().ToString();
    }

    private DataCube1<Vector, CellType> SimulateWaterfall(IEnumerable<IEnumerable<Vector>> rocks, Vector origin, bool floor)
    {
        var grid = CreateGrid(rocks);
        var maxY = rocks.Max(r => r.Max(p => p.Y)) + 1;

        while (!grid.Contains(origin))
        {
            var position = SimulateGrain(grid, origin, maxY);
            if (!floor && position.Y == maxY)
            {
                break;
            }

            grid.Set(position, CellType.Sand);
        }

        return grid;
    }

    private Vector SimulateGrain(DataCube1<Vector, CellType> grid, Vector origin, long maxY)
    {
        var position = origin;
        while (true)
        {
            var options = GetStepOptions(position);
            var newPosition = options.FirstOption(p => !grid.Contains(p)).GetOrElse(position);
            if (newPosition == position || newPosition.Y == maxY)
            {
                return newPosition;
            }

            position = newPosition;
        }
    }

    private IEnumerable<Vector> GetStepOptions(Vector position)
    {
        yield return position.AddY(1);
        yield return position.Add(new Vector(-1, 1));
        yield return position.Add(new Vector(1, 1));
    }

    private DataCube1<Vector, CellType> CreateGrid(IEnumerable<IEnumerable<Vector>> rocks)
    {
        var grid = new DataCube1<Vector, CellType>();
        foreach (var rock in rocks)
        {
            var segments = rock.Zip(rock.Skip(1));
            foreach (var (start, end) in segments)
            {
                var diff = end.Subtract(start);
                var step = diff.Sign();
                var point = start;
                while (true)
                {
                    grid.Set(point, CellType.Rock);
                    if (point == end)
                    {
                        break;
                    }
                    point = point.Add(step);
                }
            }
        }

        return grid;
    }

    private IEnumerable<Vector> ParseRocks(string line)
    {
        return line.Split(" -> ").Select(p => Vector.Parse(p.Split(",")));
    }

    private enum CellType
    {
        Rock,
        Sand
    }
}