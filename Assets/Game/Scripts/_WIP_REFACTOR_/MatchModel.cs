using System;
using System.Collections.Generic;
using System.Linq;

public class MatchModel
{
    public event Action<MatchResult> GameCompleted = delegate { };
    public event Action PlayerChanged = delegate { };
    readonly PlayerModel[] _players;
    readonly List<IGameCompletionRule> _completionRules = new();
    int _currentPlayerIndex;
    public BoardModel Board { get; }
    public PlayerModel Player { get; private set; }

    public MatchModel(PlayerModel player1, PlayerModel player2)
    {
        Board = new BoardModel();
        Board.Initialize();
        _players = new[] { player1, player2 };
        var halfCells = Board.Length * Board.Length / 2f;
        player1.TurnCount = (int) Math.Ceiling(halfCells);
        player2.TurnCount = (int) Math.Floor(halfCells);
        Player = _players[0];
    }

    public void MakeMove(CellModel cellModel)
    {
        Player.TurnCount--;
        cellModel.SetOwner(Player);
        if (!TryEndGame())
            SwitchPlayer();
    }

    public void AddRule(IGameCompletionRule rule)
    {
        _completionRules.Add(rule);
    }

    bool TryEndGame()
    {
        foreach (var condition in _completionRules)
        {
            if (!condition.IsValid(this, out var result))
                continue;

            Player.TurnCount = 0;
            if (result.VictoryCells != null)
            {
                foreach (var cell in Board.GetAllCells().Where(x => !result.VictoryCells.Contains(x)))
                    cell.SetOwner(null);
            }

            GameCompleted?.Invoke(result);
            return true;
        }

        return false;
    }

    void SwitchPlayer()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
        Player = _players[_currentPlayerIndex];
        PlayerChanged?.Invoke();
    }
}