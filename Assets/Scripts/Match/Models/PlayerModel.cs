public class PlayerModel
{
    public PlayerSymbol Symbol { get; }
    public int TurnCount { get; private set; }

    public PlayerModel(PlayerSymbol symbol)
    {
        Symbol = symbol;
    }

    public void DecreaseTurnCount()
    {
        TurnCount--;
    }

    public void ResetTurnCount()
    {
        TurnCount = 0;
    }

    public void SetTurnCount(int count)
    {
        TurnCount = count;
    }
}