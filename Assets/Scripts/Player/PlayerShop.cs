using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TheInvisibleHand.Player
{
    /// <summary>
    /// Manages the player's shop - what's for sale, pricing strategy, upgrades
    /// </summary>
    public class PlayerShop : MonoBehaviour
    {
        public static PlayerShop Instance { get; private set; }

        [Header("Shop Settings")]
        [SerializeField] private int maxDisplaySlots = 8;
        [SerializeField] private float reputationDecayRate = 0.95f;

        public float Reputation { get; private set; } = 50f; // 0-100
        public int Level { get; private set; } = 1;
        public float Experience { get; private set; }

        private Dictionary<string, ShopListing> listings = new Dictionary<string, ShopListing>();

        public event Action<string, float> OnPriceSet;
        public event Action<float> OnReputationChanged;
        public event Action<int> OnLevelUp;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Core.GameManager.Instance.OnNewDay += OnNewDay;
        }

        /// <summary>
        /// Set an item for sale at a specific price
        /// </summary>
        public void SetItemPrice(string itemId, float price)
        {
            if (!PlayerInventory.Instance.HasItem(itemId))
            {
                Debug.Log($"Cannot list {itemId} - not in inventory");
                return;
            }

            if (!listings.ContainsKey(itemId))
            {
                if (listings.Count >= maxDisplaySlots)
                {
                    Debug.Log("Shop display is full! Upgrade or remove items.");
                    return;
                }

                listings[itemId] = new ShopListing { itemId = itemId };
            }

            listings[itemId].price = price;
            listings[itemId].isActive = true;

            OnPriceSet?.Invoke(itemId, price);
            Debug.Log($"Set {itemId} price to ${price:F2}");
        }

        /// <summary>
        /// Remove item from shop display
        /// </summary>
        public void RemoveFromDisplay(string itemId)
        {
            if (listings.ContainsKey(itemId))
            {
                listings[itemId].isActive = false;
            }
        }

        /// <summary>
        /// Process a customer purchase
        /// </summary>
        public bool SellItem(string itemId, int quantity, out float totalPrice)
        {
            totalPrice = 0f;

            if (!listings.TryGetValue(itemId, out var listing) || !listing.isActive)
            {
                Debug.Log($"{itemId} is not for sale");
                return false;
            }

            if (!PlayerInventory.Instance.HasItem(itemId, quantity))
            {
                Debug.Log($"Not enough {itemId} to sell");
                return false;
            }

            totalPrice = listing.price * quantity;

            // Complete the transaction
            PlayerInventory.Instance.RemoveItem(itemId, quantity);
            PlayerInventory.Instance.AddMoney(totalPrice);

            // Update stats
            listing.totalSold += quantity;
            listing.revenue += totalPrice;

            // Track sale in market economy
            Economy.MarketEconomy.Instance.RecordSale(itemId, quantity, listing.price);

            // Gain experience and reputation
            GainExperience(totalPrice * 0.1f);
            AdjustReputation(CalculateReputationChange(itemId, listing.price));

            Debug.Log($"Sold {quantity}x {itemId} for ${totalPrice:F2}");
            return true;
        }

        /// <summary>
        /// Buy items from wholesale market
        /// </summary>
        public bool BuyWholesale(string itemId, int quantity)
        {
            float wholesalePrice = Economy.MarketEconomy.Instance.GetWholesalePrice(itemId);
            float totalCost = wholesalePrice * quantity;

            if (!PlayerInventory.Instance.CanAfford(totalCost))
            {
                Debug.Log("Cannot afford this purchase!");
                return false;
            }

            if (PlayerInventory.Instance.CurrentCapacity + quantity > PlayerInventory.Instance.MaxCapacity)
            {
                Debug.Log("Not enough inventory space!");
                return false;
            }

            PlayerInventory.Instance.RemoveMoney(totalCost);
            PlayerInventory.Instance.AddItem(itemId, quantity);

            // Track restock in market
            Economy.MarketEconomy.Instance.RecordRestock(itemId, quantity);

            Debug.Log($"Bought {quantity}x {itemId} for ${totalCost:F2} (${wholesalePrice:F2} each)");
            return true;
        }

        private float CalculateReputationChange(string itemId, float salePrice)
        {
            float marketPrice = Economy.MarketEconomy.Instance.GetCurrentPrice(itemId);
            float priceRatio = salePrice / marketPrice;

            // Fair pricing (0.9-1.1x market): +reputation
            // Overpriced (>1.3x): -reputation
            // Bargain (<0.8x): +reputation (good deal!)

            if (priceRatio > 1.3f)
                return -0.5f; // Overpriced
            else if (priceRatio > 1.1f)
                return -0.1f; // Slightly expensive
            else if (priceRatio < 0.8f)
                return 0.3f; // Great deal!
            else
                return 0.1f; // Fair price
        }

        private void AdjustReputation(float change)
        {
            Reputation = Mathf.Clamp(Reputation + change, 0f, 100f);
            OnReputationChanged?.Invoke(Reputation);
        }

        private void GainExperience(float xp)
        {
            Experience += xp;
            float xpNeeded = Level * 100f;

            if (Experience >= xpNeeded)
            {
                Level++;
                Experience -= xpNeeded;
                OnLevelUp?.Invoke(Level);
                Debug.Log($"Level up! Now level {Level}");
            }
        }

        private void OnNewDay(int day)
        {
            // Reputation decays slightly each day (encourages consistent good service)
            Reputation *= reputationDecayRate;

            // Reset daily stats
            foreach (var listing in listings.Values)
            {
                listing.dailySales = 0;
            }
        }

        public ShopListing GetListing(string itemId)
        {
            return listings.TryGetValue(itemId, out var listing) ? listing : null;
        }

        public List<ShopListing> GetAllListings()
        {
            return listings.Values.Where(l => l.isActive).ToList();
        }

        public void UpgradeDisplaySlots(int additional)
        {
            maxDisplaySlots += additional;
            Debug.Log($"Shop can now display {maxDisplaySlots} items");
        }
    }

    [Serializable]
    public class ShopListing
    {
        public string itemId;
        public float price;
        public bool isActive;
        public int totalSold;
        public float revenue;
        public int dailySales;
    }
}
