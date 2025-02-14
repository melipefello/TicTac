using System;

class MatchState : GameState
{
    readonly ServiceLocator _services;
    readonly MatchPresenter _matchPresenter;
    readonly PlayerPresenter _playerPresenter;
    readonly Action _matchExited;
    MatchModel _model;

    public MatchState(ServiceLocator services, MatchPresenter matchPresenter, PlayerPresenter playerPresenter,
        Action matchExited)
    {
        _services = services;
        _matchPresenter = matchPresenter;
        _playerPresenter = playerPresenter;
        _matchExited = matchExited;
    }

    public override void Enter()
    {
        _model = new MatchModel(new PlayerModel(PlayerSymbol.Shield), new PlayerModel(PlayerSymbol.Slash));
        _model.AddRule(new RowSequenceComplete());
        _model.AddRule(new ColumnSequenceComplete());
        _model.AddRule(new DiagonalSequenceComplete());
        _model.AddRule(new BoardFull());
        _model.PlayerChanged += OnPlayerChanged;
        _model.GameCompleted += OnGameCompleted;
        _playerPresenter.Initialize(_services, SelectCell);
        _playerPresenter.UpdatePlayer(_model.Player);
        _matchPresenter.Initialize(_services, _matchExited);
        _matchPresenter.UpdateBoard(_model.Board);
    }

    public override void Exit()
    {
        _model.PlayerChanged -= OnPlayerChanged;
        _matchPresenter.Dispose();
        _playerPresenter.Dispose();
    }

    void OnGameCompleted(MatchResult result)
    {
        _playerPresenter.UpdatePlayer(_model.Player);
    }

    void OnPlayerChanged()
    {
        _playerPresenter.UpdatePlayer(_model.Player);
    }

    void SelectCell(CellModel cellModel)
    {
        if (cellModel.IsEmpty)
            _model.MakeMove(cellModel);
    }
}