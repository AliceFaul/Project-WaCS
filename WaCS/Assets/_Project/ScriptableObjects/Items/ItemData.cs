using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/Data")]
public class ItemData : ScriptableObject
{
    public string ItemID; // ID của item
    public string DisplayName; // Tên hiển thị trong game 
    public GameObject Prefab; // Prefab của item, dùng để spawn ra trong game
    public int MaxStackSize; // Số lượng tối đa có thể xếp chồng của item
    public Sprite Icon; // Hình ảnh đại diện của item
    public float BuyPrice; // Giá của nó, update trong tương lai
    public float SellPrice; // Giá bán của nó, update trong tương lai
}
