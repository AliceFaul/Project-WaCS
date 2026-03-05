using _Project.Gameplay.Player;
using _Project.UI.Screens;
using UnityEngine;

public class UiBootstrap : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private HotbarUI hotbarUI;

    private void OnEnable()
    {
        PlayerController.OnContextInitialized += InitializeUI;
        InitializeUI();
    }

    private void OnDestroy()
    {
        PlayerController.OnContextInitialized -= InitializeUI;
    }

    private void InitializeUI()
    {
        var context = ServiceRegistry.Get<PlayerContext>();
        if (context == null)
        {
            Debug.LogError("PlayerContext not found in ServiceRegistry. Ensure PlayerController is initialized before UIBootstrap.");
            return;
        }
        inventoryUI.Init(context.Inventory);
        inventoryUI.BindInput(PlayerController.Instance);
        hotbarUI.Init(context.Hotbar);
        Debug.Log("UI initialized successfully.");
    }
}
