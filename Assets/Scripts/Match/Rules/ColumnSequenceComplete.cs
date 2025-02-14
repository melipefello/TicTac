using System.Collections.Generic;

public class ColumnSequenceComplete : SequenceComplete
{
    protected override IEnumerable<CellModel[]> GetCellSequences(BoardModel board) => board.GetColumns();
}