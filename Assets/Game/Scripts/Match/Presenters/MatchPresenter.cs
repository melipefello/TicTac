using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchPresenter : MonoBehaviour
{
    [SerializeField] Button _exitMatchButton;
    [SerializeField] AudioClip _matchMusic;
    [SerializeField] Transform _cellContainer;
    [SerializeField] CellPresenter _cellPresenterPrefab;
    [SerializeField] CanvasGroup _canvasGroup;
    readonly List<CellPresenter> _cellPresenters = new();
    ServiceLocator _services;

    public void Initialize(ServiceLocator services, Action matchExited)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _services = services;
        _exitMatchButton.onClick.AddListener(() => matchExited());
        var audioService = _services.Get<AudioService>();
        audioService.PlayMusic(_matchMusic);
    }

    public void Dispose()
    {
        _exitMatchButton.onClick.RemoveAllListeners();
        foreach (var presenter in _cellPresenters.ToList())
        {
            presenter.Dispose();
            Destroy(presenter.gameObject);
        }

        _cellPresenters.Clear();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void UpdateBoard(BoardModel board)
    {
        foreach (var presenter in _cellPresenters.ToList())
        {
            presenter.Dispose();
            Destroy(presenter.gameObject);
        }

        foreach (var cell in board.GetAllCells())
        {
            var presenter = Instantiate(_cellPresenterPrefab, _cellContainer);
            _cellPresenters.Add(presenter);
            presenter.Initialize(_services, cell);
            presenter.UpdateCell();
        }
    }
}