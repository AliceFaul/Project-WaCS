using UnityEngine;
using System;
using System.Collections.Generic;

namespace _Project.Gameplay.Player
{
    public class EquipmentItem
    {
        public string Name { get; }
        public Sprite Icon { get; }
        public Action OnUse { get; }
        public EquipmentItem(string name, Sprite icon, Action onUse)
        {
            Name = name;
            Icon = icon;
            OnUse = onUse;
        }
    }

    public class Hotbar
    {

    }
}
