using UnityEngine;
using System;

namespace TheInvisibleHand.Items
{
    /// <summary>
    /// Base item definition - represents goods that can be bought and sold
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "The Invisible Hand/Item")]
    public class Item : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemId;
        public string displayName;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;

        [Header("Economic Properties")]
        public ItemCategory category;
        public float basePrice = 1f;
        public float baseDemand = 100f;
        [Range(0f, 1f)]
        public float priceElasticity = 0.5f; // How sensitive demand is to price
        public bool isPerishable = false;
        public int shelfLife = 7; // Days before spoiling

        [Header("Gameplay")]
        public int stackSize = 99;
        public float weight = 1f;
        public Rarity rarity = Rarity.Common;

        [Header("Flavor Text")]
        [TextArea(2, 4)]
        public string[] funFacts; // Educational/fun facts about the item
        public string[] customerComments; // Things NPCs might say

        public override string ToString() => displayName;
    }

    [Serializable]
    public enum ItemCategory
    {
        Food,
        Beverage,
        Produce,
        Dairy,
        Luxury,
        Essentials,
        Seasonal,
        Crafts
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Exotic
    }
}
