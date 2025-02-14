using System.Collections.Generic;

public class DiagonalSequenceComplete : SequenceComplete
{
    protected override IEnumerable<CellModel[]> GetCellSequences(BoardModel board) => board.GetDiagonals();
}