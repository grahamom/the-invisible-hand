using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TheInvisibleHand.Economy
{
    /// <summary>
    /// Core economic simulation - handles supply, demand, and price dynamics
    /// This is where the "invisible hand" actually works!
    /// </summary>
    public class MarketEconomy : MonoBehaviour
    {
        public static MarketEconomy Instance { get; private set; }

        [Header("Market Settings")]
        [SerializeField] private float priceElasticity = 0.5f;
        [SerializeField] private float demandVolatility = 0.2f;
        [SerializeField] private float marketMemory = 0.7f; // How much yesterday affects today

        private Dictionary<string, MarketData> marketData = new Dictionary<string, MarketData>();

        public event Action<string, float> OnPriceChanged;
        public event Action<string, MarketCondition> OnMarketConditionChanged;

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
            InitializeMarket();
            Core.GameManager.Instance.OnNewDay += OnNewDay;
        }

        private void InitializeMarket()
        {
            // Initialize some basic goods
            RegisterItem("Bread", basePrice: 2.5f, baseDemand: 100f);
            RegisterItem("Milk", basePrice: 3.0f, baseDemand: 80f);
            RegisterItem("Coffee", basePrice: 4.5f, baseDemand: 120f);
            RegisterItem("Apples", basePrice: 1.5f, baseDemand: 90f);
            RegisterItem("Cheese", basePrice: 6.0f, baseDemand: 60f);
            RegisterItem("Wine", basePrice: 15.0f, baseDemand: 40f);
            RegisterItem("Flowers", basePrice: 8.0f, baseDemand: 50f);
        }

        public void RegisterItem(string itemId, float basePrice, float baseDemand)
        {
            if (marketData.ContainsKey(itemId))
            {
                Debug.LogWarning($"Item {itemId} already registered");
                return;
            }

            marketData[itemId] = new MarketData
            {
                ItemId = itemId,
                BasePrice = basePrice,
                CurrentPrice = basePrice,
                BaseDemand = baseDemand,
                CurrentDemand = baseDemand,
                Supply = baseDemand * 1.2f, // Slightly oversupplied initially
                Condition = MarketCondition.Stable
            };
        }

        /// <summary>
        /// Calculate current market price based on supply and demand
        /// Uses basic economic formula: P = BasePrice * (Demand/Supply)^elasticity
        /// </summary>
        public float GetCurrentPrice(string itemId)
        {
            if (!marketData.TryGetValue(itemId, out var data))
            {
                Debug.LogError($"Item {itemId} not found in market");
                return 0f;
            }

            return data.CurrentPrice;
        }

        public float GetWholesalePrice(string itemId)
        {
            // Wholesale is typically 60-70% of retail
            return GetCurrentPrice(itemId) * 0.65f;
        }

        /// <summary>
        /// Record a sale - affects supply and potentially demand
        /// </summary>
        public void RecordSale(string itemId, int quantity, float pricePerUnit)
        {
            if (!marketData.TryGetValue(itemId, out var data))
                return;

            // Decrease supply
            data.Supply = Mathf.Max(0, data.Supply - quantity);

            // If price is high, demand might decrease (price elasticity)
            if (pricePerUnit > data.BasePrice * 1.3f)
            {
                data.CurrentDemand *= (1f - priceElasticity * 0.1f);
            }
            else if (pricePerUnit < data.BasePrice * 0.7f)
            {
                // Low prices increase demand
                data.CurrentDemand *= (1f + priceElasticity * 0.1f);
            }

            RecalculatePrice(itemId);
        }

        /// <summary>
        /// Record a restock - increases supply
        /// </summary>
        public void RecordRestock(string itemId, int quantity)
        {
            if (!marketData.TryGetValue(itemId, out var data))
                return;

            data.Supply += quantity;
            RecalculatePrice(itemId);
        }

        private void RecalculatePrice(string itemId)
        {
            if (!marketData.TryGetValue(itemId, out var data))
                return;

            float oldPrice = data.CurrentPrice;

            // Economic formula: Price influenced by demand/supply ratio
            float supplyDemandRatio = data.Supply > 0
                ? data.CurrentDemand / data.Supply
                : 2f; // Scarcity!

            float priceMultiplier = Mathf.Pow(supplyDemandRatio, priceElasticity);
            data.CurrentPrice = data.BasePrice * priceMultiplier;

            // Clamp to reasonable bounds (50% to 300% of base)
            data.CurrentPrice = Mathf.Clamp(data.CurrentPrice,
                data.BasePrice * 0.5f,
                data.BasePrice * 3f);

            // Update market condition
            UpdateMarketCondition(itemId);

            if (Mathf.Abs(oldPrice - data.CurrentPrice) > 0.01f)
            {
                OnPriceChanged?.Invoke(itemId, data.CurrentPrice);
            }
        }

        private void UpdateMarketCondition(string itemId)
        {
            if (!marketData.TryGetValue(itemId, out var data))
                return;

            MarketCondition oldCondition = data.Condition;

            float priceRatio = data.CurrentPrice / data.BasePrice;
            float supplyRatio = data.Supply / data.BaseDemand;

            if (priceRatio > 1.5f || supplyRatio < 0.3f)
                data.Condition = MarketCondition.Shortage;
            else if (priceRatio < 0.7f || supplyRatio > 2.0f)
                data.Condition = MarketCondition.Surplus;
            else if (priceRatio > 1.2f)
                data.Condition = MarketCondition.Rising;
            else if (priceRatio < 0.85f)
                data.Condition = MarketCondition.Falling;
            else
                data.Condition = MarketCondition.Stable;

            if (oldCondition != data.Condition)
            {
                OnMarketConditionChanged?.Invoke(itemId, data.Condition);
            }
        }

        private void OnNewDay(int day)
        {
            // Daily market simulation
            foreach (var item in marketData.Keys.ToList())
            {
                SimulateDailyMarket(item);
            }
        }

        private void SimulateDailyMarket(string itemId)
        {
            if (!marketData.TryGetValue(itemId, out var data))
                return;

            // Demand fluctuation (random events, seasons, trends)
            float demandChange = UnityEngine.Random.Range(-demandVolatility, demandVolatility);
            data.CurrentDemand = Mathf.Lerp(
                data.BaseDemand,
                data.CurrentDemand * (1f + demandChange),
                marketMemory
            );

            // Supply naturally replenishes a bit (other merchants restock)
            data.Supply += data.BaseDemand * 0.3f;

            // Some supply gets consumed by the market
            float marketConsumption = data.CurrentDemand * 0.5f;
            data.Supply = Mathf.Max(0, data.Supply - marketConsumption);

            RecalculatePrice(itemId);
        }

        public MarketData GetMarketData(string itemId)
        {
            return marketData.TryGetValue(itemId, out var data) ? data : null;
        }

        public Dictionary<string, MarketData> GetAllMarketData()
        {
            return new Dictionary<string, MarketData>(marketData);
        }
    }

    [Serializable]
    public class MarketData
    {
        public string ItemId;
        public float BasePrice;
        public float CurrentPrice;
        public float BaseDemand;
        public float CurrentDemand;
        public float Supply;
        public MarketCondition Condition;
    }

    public enum MarketCondition
    {
        Shortage,   // High prices, low supply
        Rising,     // Prices trending up
        Stable,     // Balanced market
        Falling,    // Prices trending down
        Surplus     // Low prices, high supply
    }
}
