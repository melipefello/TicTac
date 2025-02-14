using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using static PrimeTween.Tween;
using Color = UnityEngine.Color;

public class IntroPresenter : MonoBehaviour
{
    [SerializeField] Image _background;
    [SerializeField] Image _overlay;
    [SerializeField] Transform _titleRoot;
    [SerializeField] float _secondsToReadTitle = 3;
    [SerializeField] TweenSettings _loadingSettings;
    [SerializeField] TweenSettings _loadCompletedSettings;
    [SerializeField] AudioClip _introMusic;
    [SerializeField] AudioClip _titleReveal;
    [SerializeField] CanvasGroup _canvasGroup;
    Sequence _introSequence;
    Action _introCompleted;

    public async void Initialize(Action introCompleted, ServiceLocator services)
    {
        _introCompleted = introCompleted;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        var audioService = services.Get<AudioService>();
        audioService.PlayMusic(_introMusic);
        _introSequence.Complete();
        _introSequence = Sequence.Create()
            .Group(Color(_background, Color.black, _loadingSettings))
            .Chain(Color(_background, Color.white, _loadCompletedSettings));

        await _introSequence;
        _titleRoot.gameObject.SetActive(true);
        audioService.StopMusic();
        audioService.PlayClip(_titleReveal);
        _introSequence = Sequence.Create()
            .Chain(ShakeScale(_titleRoot, Vector3.up, .3f))
            .Chain(Delay(_secondsToReadTitle))
            .Chain(Alpha(_overlay, 1, _loadCompletedSettings))
            .Chain(Delay(0.3f));

        await _introSequence;
        _introCompleted?.Invoke();
    }

    public void Dispose()
    {
        _introSequence.Complete();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}