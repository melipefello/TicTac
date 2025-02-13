using System;
using System.Collections.Generic;
using System.Linq;

public class BoardModel
{
    public readonly int Length = 3;
    readonly CellModel[][] _cells;
    public bool HasEmptyCells => GetAllCells().Any(cell => cell.IsEmpty);

    public BoardModel()
    {
        _cells = new CellModel[Length][];
    }

    public IEnumerable<CellModel> GetAllCells()
    {
        return _cells.SelectMany(row => row).ToArray();
    }

    public CellModel[][] GetRows() => _cells;

    public CellModel[][] GetColumns()
    {
        return Enumerable.Range(0, Length)
            .Select(columnIndex => _cells.Select(row => row[columnIndex]).ToArray())
            .ToArray();
    }

    public CellModel[][] GetDiagonals()
    {
        var mainDiagonal = Enumerable.Range(0, Math.Min(Length, Length)).Select(i => _cells[i][i]);
        var antiDiagonal = Enumerable.Range(0, Math.Min(Length, Length)).Select(i => _cells[i][Length - 1 - i]);
        return new[] { mainDiagonal.ToArray(), antiDiagonal.ToArray() };
    }

    public void Initialize()
    {
        for (var i = 0; i < Length; i++)
        {
            _cells[i] = new CellModel[Length];
            for (var j = 0; j < Length; j++)
            {
                _cells[i][j] = new CellModel();
            }
        }
    }
}