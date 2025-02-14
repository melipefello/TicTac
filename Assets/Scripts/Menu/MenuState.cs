using System;

class MenuState : GameState
{
    readonly ServiceLocator _services;
    readonly MenuPresenter _menuPresenter;
    readonly Action _matchStarted;
    readonly Action _gameExited;

    public MenuState(ServiceLocator services, MenuPresenter menuPresenter, Action matchStarted, Action gameExited)
    {
        _services = services;
        _menuPresenter = menuPresenter;
        _matchStarted = matchStarted;
        _gameExited = gameExited;
    }

    public override void Enter()
    {
        _menuPresenter.Initialize(_services, _matchStarted, _gameExited);
    }

    public override void Exit()
    {
        _menuPresenter.Dispose();
    }
}