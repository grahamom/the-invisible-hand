using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TheInvisibleHand.Player
{
    /// <summary>
    /// Manages player's inventory and money
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance { get; private set; }

        [Header("Starting Resources")]
        [SerializeField] private float startingMoney = 100f;
        [SerializeField] private int inventoryCapacity = 100;

        public float Money { get; private set; }
        public int CurrentCapacity => inventory.Sum(i => i.quantity);
        public int MaxCapacity => inventoryCapacity;

        private List<InventorySlot> inventory = new List<InventorySlot>();

        public event Action<float> OnMoneyChanged;
        public event Action<InventorySlot> OnItemAdded;
        public event Action<InventorySlot> OnItemRemoved;
        public event Action<string, int> OnItemQuantityChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Money = startingMoney;
        }

        public bool AddItem(string itemId, int quantity)
        {
            if (CurrentCapacity + quantity > MaxCapacity)
            {
                Debug.Log("Not enough inventory space!");
                return false;
            }

            var existingSlot = inventory.FirstOrDefault(i => i.itemId == itemId);
            if (existingSlot != null)
            {
                existingSlot.quantity += quantity;
                OnItemQuantityChanged?.Invoke(itemId, existingSlot.quantity);
            }
            else
            {
                var newSlot = new InventorySlot
                {
                    itemId = itemId,
                    quantity = quantity,
                    acquiredDay = Core.GameManager.Instance.CurrentDay
                };
                inventory.Add(newSlot);
                OnItemAdded?.Invoke(newSlot);
            }

            return true;
        }

        public bool RemoveItem(string itemId, int quantity)
        {
            var slot = inventory.FirstOrDefault(i => i.itemId == itemId);
            if (slot == null || slot.quantity < quantity)
            {
                Debug.Log($"Not enough {itemId} in inventory");
                return false;
            }

            slot.quantity -= quantity;
            OnItemQuantityChanged?.Invoke(itemId, slot.quantity);

            if (slot.quantity <= 0)
            {
                inventory.Remove(slot);
                OnItemRemoved?.Invoke(slot);
            }

            return true;
        }

        public int GetItemQuantity(string itemId)
        {
            var slot = inventory.FirstOrDefault(i => i.itemId == itemId);
            return slot?.quantity ?? 0;
        }

        public bool HasItem(string itemId, int quantity = 1)
        {
            return GetItemQuantity(itemId) >= quantity;
        }

        public bool AddMoney(float amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
            return true;
        }

        public bool RemoveMoney(float amount)
        {
            if (Money < amount)
            {
                Debug.Log("Not enough money!");
                return false;
            }

            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
            return true;
        }

        public bool CanAfford(float amount) => Money >= amount;

        public List<InventorySlot> GetAllItems() => new List<InventorySlot>(inventory);

        public void UpgradeCapacity(int additionalSlots)
        {
            inventoryCapacity += additionalSlots;
            Debug.Log($"Inventory capacity increased to {inventoryCapacity}");
        }

        /// <summary>
        /// Check for spoiled items (if perishable)
        /// </summary>
        public void CheckForSpoilage()
        {
            int currentDay = Core.GameManager.Instance.CurrentDay;
            var spoiledItems = inventory.Where(slot =>
            {
                // This would check against item's shelf life
                // For now, simplified: items older than 5 days
                return (currentDay - slot.acquiredDay) > 5;
            }).ToList();

            foreach (var slot in spoiledItems)
            {
                Debug.Log($"{slot.itemId} has spoiled!");
                inventory.Remove(slot);
                OnItemRemoved?.Invoke(slot);
            }
        }
    }

    [Serializable]
    public class InventorySlot
    {
        public string itemId;
        public int quantity;
        public int acquiredDay;
        public float purchasePrice; // Track buy price for profit calculation
    }
}
