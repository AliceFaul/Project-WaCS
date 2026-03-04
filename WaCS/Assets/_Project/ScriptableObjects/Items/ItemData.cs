using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/Data")]
public class ItemData : ScriptableObject
{
    public string ItemID; // ID của item
    public string DisplayName; // Tên hiển thị trong game 
    public Sprite Icon; // Hình ảnh đại diện của item
    public float BuyPrice; // Giá của nó, update trong tương lai
    public float SellPrice; // Giá bán của nó, update trong tương lai
}
