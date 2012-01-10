using System;
using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife
{
    internal class ConwaysGame
    {
        private static void Main(string[] args)
        {
        }
    }

    public class GridParser
    {
        public Grid Parse(string input)
        {
            var lines = input.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            var dimensions = lines.Skip(1).First().Split(' ').Select(x => Convert.ToInt32(x)).ToList();

            return new Grid(
                generation: Convert.ToInt32(lines.First().Replace("Generation ", "").Replace(":", "")),
                cells:
                    lines.Skip(2).Select(
                        (line, row) => line.Select((c, col) => new Cell(row, col, c == '*')).Where(c => c.Alive)).
                    SelectMany(x => x),
                rowCount: dimensions.First(),
                colCount: dimensions.Last());
        }
    }

    public class Grid
    {
        private readonly IEnumerable<Cell> _cells;
        private readonly int _generation;
        private readonly int _rowCount;
        private readonly int _colCount;

        public Grid(int generation, IEnumerable<Cell> cells, int rowCount, int colCount)
        {
            _generation = generation;
            _cells = cells;
            _rowCount = rowCount;
            _colCount = colCount;
        }

        public bool IsAlive(int x, int y)
        {
            return _cells.FirstOrDefault(c => c.X == x && c.Y == y).Alive;
        }

        public Grid Tick()
        {
            return new Grid(_generation + 1, NextGeneration(), _rowCount, _colCount);
        }

        private IEnumerable<Cell> NextGeneration()
        {
            return Enumerable.Range(0, _rowCount).
                Select(row => Enumerable.Range(0, _colCount).
                                  Where(col => WillLive(row, col)).
                                  Select(col => new Cell(row, col, true))).SelectMany(x => x);
        }

        private bool WillLive(int row, int col)
        {
            return (!IsAlive(row, col) && CountLiveNeighbours(row, col) == 3)
                   ||
                   (IsAlive(row, col) && CountLiveNeighbours(row, col) == 3 || CountLiveNeighbours(row, col) == 2);
        }

        private int CountLiveNeighbours(int row, int col)
        {
            var count = 0;
            if (IsAlive(row + 1, col)) count++;
            if (IsAlive(row + 1, col + 1)) count++;
            if (IsAlive(row + 1, col - 1)) count++;
            if (IsAlive(row, col + 1)) count++;
            if (IsAlive(row, col - 1)) count++;
            if (IsAlive(row - 1, col + 1)) count++;
            if (IsAlive(row - 1, col - 1)) count++;
            if (IsAlive(row - 1, col)) count++;

            return count;
        }
    }

    public struct Cell
    {
        public readonly int X, Y;
        public readonly bool Alive;

        public Cell(int x, int y, bool alive)
        {
            X = x;
            Y = y;
            Alive = alive;
        }
    }
}