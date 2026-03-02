using UnityEngine;

public interface IPlayerInput
{
    Vector2 Move { get; set; }
    Vector2 Look { get; set; }
    bool Interact { get; }
    bool SecondaryInteract { get; }
    bool Inventory { get; }
    bool Escape { get; }

    bool HotbarNext { get; }
    bool HotbarPrevious { get; }
    int HotbarNumberPressed { get; }
}