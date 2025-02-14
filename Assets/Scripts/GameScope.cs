using UnityEngine;

public class GameScope : MonoBehaviour
{
    [SerializeField] IntroPresenter _introPresenter;
    [SerializeField] MenuPresenter _menuPresenter;
    [SerializeField] MatchPresenter _matchPresenter;
    [SerializeField] PlayerPresenter _playerPresenter;
    GameState _state;
    ServiceLocator _services;

    void Start()
    {
        _services = new ServiceLocator();
        _services.Register(() => new AudioService());
        SetState(new IntroState(_services, _introPresenter, EnterMenu));
    }

    void EnterMenu()
    {
        SetState(new MenuState(_services, _menuPresenter, EnterMatch, ExitGame));
    }

    void EnterMatch()
    {
        SetState(new MatchState(_services, _matchPresenter, _playerPresenter, EnterMenu));
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void SetState(GameState state)
    {
        _state?.Exit();
        _state = state;
        _state.Enter();
    }
}