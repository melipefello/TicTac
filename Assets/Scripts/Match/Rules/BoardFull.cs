public class BoardFull : IGameCompletionRule
{
    public bool IsValid(MatchModel match, out MatchResult result)
    {
        result = null;
        if (match.Board.HasEmptyCells)
            return false;

        result = new MatchResult(null);
        return true;
    }
}