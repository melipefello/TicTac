public interface IGameCompletionRule
{
    bool IsValid(MatchModel match, out MatchResult result);
}