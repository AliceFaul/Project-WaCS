using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/Data")]
public class ItemData : ScriptableObject
{
    public string ItemID; // ID của item
    public string DisplayName; // Tên hiển thị trong game 
    public float Price; // Giá của nó, update trong tương lai
}
