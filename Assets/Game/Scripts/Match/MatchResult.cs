using System.Collections.Generic;

public class MatchResult
{
    public IEnumerable<CellModel> VictoryCells { get; }

    public MatchResult(IEnumerable<CellModel> victoryCells)
    {
        VictoryCells = victoryCells;
    }
}