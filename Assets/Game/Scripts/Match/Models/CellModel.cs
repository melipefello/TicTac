using System;

public class CellModel
{
    public event Action OwnerChanged = delegate { };
    public bool IsEmpty => Owner == null;
    public PlayerModel Owner { get; private set; }

    public void SetOwner(PlayerModel playerModel)
    {
        Owner = playerModel;
        OwnerChanged?.Invoke();
    }
}