using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/ItemDictionary")]
public class ItemDictionary : ScriptableObject
{
    [SerializeField] private List<ItemData> items = new();

    private Dictionary<string, ItemData> _itemLookup;

    private static ItemDictionary _instance;
    public static ItemDictionary Instance => _instance;

    public void Initialize()
    {
        if (_itemLookup != null)
            return;

        _itemLookup = new Dictionary<string, ItemData>();

        foreach (var item in items)
        {
            if (item == null)
            {
                Debug.LogWarning("Null item in ItemDatabase.");
                continue;
            }

            if (string.IsNullOrEmpty(item.ItemID))
            {
                Debug.LogError($"Item '{item.name}' has empty ItemID.");
                continue;
            }

            if (_itemLookup.ContainsKey(item.ItemID))
            {
                Debug.LogError($"Duplicate ItemID detected: {item.ItemID}");
                continue;
            }

            _itemLookup.Add(item.ItemID, item);
        }
    }

    public ItemData GetById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        if (_itemLookup == null)
            Initialize();

        if (_itemLookup.TryGetValue(id, out var item))
            return item;

        Debug.LogWarning($"ItemID not found in database: {id}");
        return null;
    }

    public IReadOnlyList<ItemData> GetAllItems()
    {
        return items;
    }

    // Inject instance at runtime
    public static void SetInstance(ItemDictionary database)
    {
        _instance = database;
        _instance.Initialize();
    }
}
