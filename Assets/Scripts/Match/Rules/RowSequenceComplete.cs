using System.Collections.Generic;

public class RowSequenceComplete : SequenceComplete
{
    protected override IEnumerable<CellModel[]> GetCellSequences(BoardModel board) => board.GetRows();
}