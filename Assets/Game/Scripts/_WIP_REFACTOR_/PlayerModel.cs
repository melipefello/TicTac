public class PlayerModel
{
    public PlayerSymbol Symbol { get; }
    public int TurnCount { get; set; }

    public PlayerModel(PlayerSymbol symbol)
    {
        Symbol = symbol;
    }
}