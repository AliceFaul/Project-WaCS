using UnityEngine;

public interface IPlayerInput
{
    Vector2 Move { get; set; }
    Vector2 Look { get; set; }
    bool Interact { get; }
}