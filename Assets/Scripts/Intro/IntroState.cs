using System;

class IntroState : GameState
{
    readonly ServiceLocator _services;
    readonly IntroPresenter _presenter;
    readonly Action _introCompleted;

    public IntroState(ServiceLocator services, IntroPresenter presenter, Action introCompleted)
    {
        _services = services;
        _presenter = presenter;
        _introCompleted = introCompleted;
    }

    public override void Enter()
    {
        _presenter.Initialize(_introCompleted, _services);
    }

    public override void Exit()
    {
        _presenter.Dispose();
    }
}