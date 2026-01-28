using System.Collections.Generic;
using System.Linq;

namespace _Project.Gameplay.Customer
{
    public struct CartItem
    {
        public string ItemId;
        public float Price;
    }
    
    public class Cart
    {
        private readonly List<CartItem> _items = new();
        
        public IReadOnlyList<CartItem> Items => _items;
        public float TotalPrice => _items.Sum(i => i.Price);

        public void AddItem(CartItem item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}