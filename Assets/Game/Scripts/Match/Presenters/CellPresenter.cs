using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

class CellPresenter : MonoBehaviour
{
    [SerializeField] AudioClip _cellSpawnClip;
    [SerializeField] Sprite _slashIcon;
    [SerializeField] Sprite _shieldIcon;
    [SerializeField] Image _cardImage;
    ServiceLocator _services;
    Tween _tween;
    public CellModel Cell { get; private set; }

    public void Initialize(ServiceLocator services, CellModel cell)
    {
        transform.localScale = Vector3.zero;
        _services = services;
        Cell = cell;
        Cell.OwnerChanged += UpdateCell;
    }

    public void Dispose()
    {
        Cell.OwnerChanged -= UpdateCell;
        _tween.Complete();
    }

    public void UpdateCell()
    {
        var audioService = _services.Get<AudioService>();
        audioService.PlayClip(_cellSpawnClip);
        _cardImage.enabled = false;
        if (!Cell.IsEmpty)
        {
            _cardImage.enabled = true;
            _cardImage.sprite = Cell.Owner.Symbol switch
            {
                PlayerSymbol.Slash => _slashIcon,
                PlayerSymbol.Shield => _shieldIcon,
                _ => null,
            };
        }

        _tween.Complete();
        _tween = Tween.Scale(transform, Vector3.zero, Vector3.one, 0.3f, Ease.OutBack);
    }
}