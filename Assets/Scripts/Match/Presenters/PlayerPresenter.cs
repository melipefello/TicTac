using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class PlayerPresenter : MonoBehaviour
{
    [SerializeField] Transform _cardContainer;
    [SerializeField] CardPresenter _cardPresenterPrefab;
    [SerializeField] AudioClip _slashClip;
    [SerializeField] AudioClip _shieldClip;
    [SerializeField] CanvasGroup _canvasGroup;
    readonly List<CardPresenter> _cardPresenters = new();
    Action<CellModel> _selectedCell;
    ServiceLocator _services;

    public void Initialize(ServiceLocator services, Action<CellModel> selectedCell)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _services = services;
        _selectedCell = selectedCell;
    }

    public void Dispose()
    {
        foreach (var presenter in _cardPresenters)
        {
            presenter.Dispose();
            Destroy(presenter.gameObject);
        }

        _cardPresenters.Clear();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void UpdatePlayer(PlayerModel player)
    {
        var playerIntroClip = player.Symbol == PlayerSymbol.Slash ? _slashClip : _shieldClip;
        var audioService = _services.Get<AudioService>();
        audioService.PlayClip(playerIntroClip);
        foreach (var presenter in _cardPresenters)
        {
            presenter.Dispose();
            Destroy(presenter.gameObject);
        }

        _cardPresenters.Clear();
        for (var i = 0; i < player.TurnCount; i++)
        {
            var presenter = Instantiate(_cardPresenterPrefab, _cardContainer);
            presenter.Initialize(_services, OnCardReleased);
            presenter.UpdateSymbol(player.Symbol);
            _cardPresenters.Add(presenter);
        }
    }

    void OnCardReleased(PointerEventData eventData)
    {
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (var hit in hits)
        {
            if (!hit.gameObject.TryGetComponent(out CellPresenter presenter))
                continue;

            _selectedCell?.Invoke(presenter.Cell);
            break;
        }
    }
}