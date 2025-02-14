using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PrimeTween.Tween;
using static UnityEngine.Mathf;

class CardPresenter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] Image _cardImage;
    [SerializeField] Sprite _slashIcon;
    [SerializeField] Sprite _shieldIcon;
    [SerializeField] AudioClip _cardClip;
    [SerializeField] AudioClip _pickUpClip;
    [SerializeField] AudioClip _releaseClip;
    [SerializeField] AudioSource _dragAmbience;
    [SerializeField] Transform _cardRoot;
    [SerializeField] Transform _cardImageRoot;
    ServiceLocator _services;
    Tween _scaleTween;
    Sequence _sequence;
    Action<PointerEventData> _onCardReleased;

    public void Initialize(ServiceLocator services, Action<PointerEventData> onCardReleased)
    {
        _onCardReleased = onCardReleased;
        _services = services;
    }

    public void Dispose()
    {
        _scaleTween.Complete();
        _sequence.Complete();
    }

    public void UpdateSymbol(PlayerSymbol symbol)
    {
        var audioService = _services.Get<AudioService>();
        audioService.PlayClip(_cardClip);
        _cardImage.sprite = symbol switch
        {
            PlayerSymbol.Slash => _slashIcon,
            PlayerSymbol.Shield => _shieldIcon,
            _ => null,
        };

        _scaleTween.Complete();
        _scaleTween = Scale(_cardImageRoot, Vector3.zero, Vector3.one, 0.3f, Ease.OutBack);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragAmbience.Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var angleStep = -eventData.delta.x * 1.5f;
        var zAngle = LerpAngle(_cardImageRoot.localEulerAngles.z, angleStep, 0.2f);
        var yAngle = LerpAngle(_cardImageRoot.localEulerAngles.y, angleStep * 1.5f, 0.2f);
        _cardImageRoot.localEulerAngles = new Vector3(0, yAngle, zAngle);
        _cardRoot.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardImageRoot.localEulerAngles = Vector3.zero;
        _cardRoot.localPosition = Vector3.zero;
        _dragAmbience.Stop();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var audioService = _services.Get<AudioService>();
        audioService.PlayClip(_pickUpClip);
        _sequence.Complete();
        _sequence = Sequence.Create()
            .Group(Scale(_cardRoot, 1.5f, 0.1f))
            .Group(LocalPosition(_cardImageRoot, Vector3.down * 24, 0.1f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var audioService = _services.Get<AudioService>();
        audioService.PlayClip(_releaseClip);
        _sequence.Complete();
        _sequence = Sequence.Create()
            .Group(Scale(_cardRoot, 1f, 0.1f))
            .Chain(LocalPosition(_cardImageRoot, Vector3.zero, 0.2f, Ease.OutBounce));

        _onCardReleased?.Invoke(eventData);
    }
}