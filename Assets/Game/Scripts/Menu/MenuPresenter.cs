using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

class MenuPresenter : MonoBehaviour
{
    [SerializeField] Button _startLocalMultiplayerButton;
    [SerializeField] Button _startSinglePlayerButton;
    [SerializeField] Button _quitGameButton;
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _buttonVisible;
    [SerializeField] CanvasGroup _canvasGroup;
    Tween _buttonTween;

    public async void Initialize(ServiceLocator services, Action matchStarted, Action gameExited)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _startLocalMultiplayerButton.onClick.AddListener(() => matchStarted());
        _quitGameButton.onClick.AddListener(() => gameExited());
        var audioService = services.Get<AudioService>();
        audioService.PlayMusic(_menuMusic);
        await SpawnButton(audioService, _startLocalMultiplayerButton.transform);
        await SpawnButton(audioService, _startSinglePlayerButton.transform);
        await SpawnButton(audioService, _quitGameButton.transform);
    }

    public void Dispose()
    {
        _startLocalMultiplayerButton.onClick.RemoveAllListeners();
        _quitGameButton.onClick.RemoveAllListeners();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _buttonTween.Complete();
    }

    async Task SpawnButton(AudioService audioService, Transform target)
    {
        audioService.PlayClip(_buttonVisible);
        _buttonTween = Tween.Scale(target, Vector3.zero, Vector3.one, 0.3f, Ease.OutBack);
        await _buttonTween;
    }
}