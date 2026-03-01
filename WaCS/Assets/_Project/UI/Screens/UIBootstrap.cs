using _Project.Gameplay.Player;
using UnityEngine;

namespace _Project.UI.Screens
{
    public class UIBootstrap : MonoBehaviour
    {
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private HotbarUI hotbarUI;

        private void OnEnable()
        {
            PlayerController.OnContextInitialized += InitializeUI;
        }

        private void OnDestroy()
        {
            PlayerController.OnContextInitialized -= InitializeUI;
        }

        private void InitializeUI(PlayerContext context)
        {
            inventoryUI.Init(context.Inventory);
            hotbarUI.Init(context.Hotbar);
        }
    }
}
