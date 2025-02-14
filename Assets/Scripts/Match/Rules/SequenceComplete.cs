using System.Collections.Generic;
using System.Linq;

public abstract class SequenceComplete : IGameCompletionRule
{
    public bool IsValid(MatchModel match, out MatchResult result)
    {
        result = null;
        foreach (var sequence in GetCellSequences(match.Board))
        {
            var isComplete = sequence.All(cell => !cell.IsEmpty);
            if (!isComplete)
                continue;

            var areEqual = sequence.All(cell => cell.Owner == sequence[0].Owner);
            if (!areEqual)
                continue;

            result = new MatchResult(sequence);
            return true;
        }

        return false;
    }

    protected abstract IEnumerable<CellModel[]> GetCellSequences(BoardModel board);
}