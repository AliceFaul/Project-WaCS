using _Project.Gameplay.Player;
using _Project.Systems.Game;
using UnityEngine;

public class CheckoutItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    private bool _isScanned;

    public bool CanInteract(PlayerContext context)
    {
        return !_isScanned;
    }

    public string GetPrompt()
    {
        return $"Press E to Scan";
    }

    public void Interact(PlayerContext context)
    {
        var checkout = ServiceRegistry.Get<CheckoutSystem>();
        if(checkout.TryScanItem(itemData))
        {
            _isScanned = true;
            gameObject.SetActive(false); // TODO: Add Animation
        }
    }
}
