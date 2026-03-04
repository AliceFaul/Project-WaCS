using UnityEditor;
using UnityEngine;
using _Project.Gameplay.Player;

public class InventoryEditorWindow : EditorWindow
{
    private Inventory _inventory;
    private ItemData _itemToAdd;
    private int _quantity = 1;

    [MenuItem("Supermarket/Inventory Editor")]
    public static void ShowWindow()
    {
        GetWindow<InventoryEditorWindow>("Inventory Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add Item To Player Inventory", EditorStyles.boldLabel);

        // Nút Refresh luôn hiển thị
        if (GUILayout.Button("Refresh Inventory Reference"))
        {
            _inventory = null;
            TryFindInventory();
        }

        // Nếu chưa có inventory, thử tìm lại
        if (_inventory == null)
        {
            TryFindInventory();
        }

        if (_inventory == null)
        {
            EditorGUILayout.HelpBox("Không tìm thấy Inventory trong scene. Hãy chắc chắn Player đã spawn!", MessageType.Warning);
            return;
        }

        _itemToAdd = (ItemData)EditorGUILayout.ObjectField("Item", _itemToAdd, typeof(ItemData), false);
        _quantity = EditorGUILayout.IntField("Quantity", _quantity);

        EditorGUI.BeginDisabledGroup(_itemToAdd == null || _quantity <= 0);
        if (GUILayout.Button("Add To Inventory"))
        {
            _inventory.AddItem(_itemToAdd, _quantity);
            Debug.Log($"Đã thêm {_quantity} x {_itemToAdd.DisplayName} vào inventory.");
        }
        EditorGUI.EndDisabledGroup();
    }

    private void TryFindInventory()
    {
        var context = ServiceRegistry.Get<PlayerContext>();
        if (context != null)
        {
            _inventory = context.Inventory;
        }
    }
}