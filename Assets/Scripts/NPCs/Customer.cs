using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TheInvisibleHand.NPCs
{
    /// <summary>
    /// Individual customer AI - each has unique preferences, budgets, and personalities
    /// Their behavior demonstrates economic principles naturally
    /// </summary>
    public class Customer : MonoBehaviour
    {
        [Header("Identity")]
        public string customerName;
        public CustomerArchetype archetype;
        public Sprite portrait;

        [Header("Economic Behavior")]
        [Range(0f, 1f)]
        public float priceAwareness = 0.5f; // How well they know market prices
        [Range(0f, 1f)]
        public float priceSensitivity = 0.5f; // How much they care about price
        public float budget;
        public float dailyBudget;

        [Header("Preferences")]
        public Dictionary<string, float> itemPreferences = new Dictionary<string, float>(); // -1 to 1
        public List<string> shoppingList = new List<string>();

        [Header("Personality")]
        public Mood currentMood = Mood.Neutral;
        public float patience = 5f; // Seconds they'll wait
        public bool isImpulsive = false;
        public bool isLoyal = false; // Will they return?

        private float reputationThreshold = 30f; // Won't shop if reputation too low
        private bool hasShoppedToday = false;

        public event Action<Customer, string, int> OnPurchaseAttempt;
        public event Action<Customer, string> OnComplaint;
        public event Action<Customer> OnLeaving;

        private void Start()
        {
            if (string.IsNullOrEmpty(customerName))
            {
                customerName = GenerateRandomName();
            }

            GenerateShoppingList();
            budget = dailyBudget;
        }

        /// <summary>
        /// Customer evaluates whether to buy an item
        /// This is where economic decision-making happens!
        /// </summary>
        public bool EvaluatePurchase(string itemId, float askingPrice, out int quantityToBuy)
        {
            quantityToBuy = 0;

            // Check if they even want this item
            if (!shoppingList.Contains(itemId) && !isImpulsive)
            {
                return false;
            }

            // Get market data
            var marketPrice = Economy.MarketEconomy.Instance.GetCurrentPrice(itemId);
            var marketCondition = Economy.MarketEconomy.Instance.GetMarketData(itemId)?.Condition;

            // Calculate perceived value
            float priceRatio = askingPrice / marketPrice;
            float perceivedValue = CalculatePerceivedValue(itemId, askingPrice, marketPrice);

            // Decision factors
            bool tooExpensive = priceRatio > (1f + priceSensitivity);
            bool canAfford = budget >= askingPrice;
            bool goodDeal = priceRatio < 0.9f;
            bool desperate = marketCondition == Economy.MarketCondition.Shortage && shoppingList.Contains(itemId);

            // Decision logic
            if (!canAfford)
            {
                React("Can't afford that...", Mood.Disappointed);
                return false;
            }

            if (tooExpensive && !desperate)
            {
                React($"${askingPrice:F2}?! That's a ripoff!", Mood.Angry);
                OnComplaint?.Invoke(this, itemId);
                return false;
            }

            if (goodDeal || desperate || perceivedValue > 0.7f)
            {
                // Calculate quantity
                quantityToBuy = CalculatePurchaseQuantity(itemId, askingPrice);

                if (goodDeal)
                    React("What a bargain!", Mood.Happy);
                else if (desperate)
                    React("I really need this...", Mood.Stressed);

                return quantityToBuy > 0;
            }

            return false;
        }

        private float CalculatePerceivedValue(string itemId, float askingPrice, float marketPrice)
        {
            float value = 1f;

            // Personal preference
            if (itemPreferences.TryGetValue(itemId, out float preference))
            {
                value += preference * 0.3f;
            }

            // Price comparison (if they're aware)
            if (priceAwareness > 0.5f)
            {
                float priceRatio = askingPrice / marketPrice;
                value *= (2f - priceRatio); // Lower price = higher value
            }

            // Urgency (if it's on their shopping list)
            if (shoppingList.Contains(itemId))
            {
                value += 0.4f;
            }

            // Mood affects perception
            value *= GetMoodMultiplier();

            return Mathf.Clamp01(value);
        }

        private int CalculatePurchaseQuantity(string itemId, float pricePerUnit)
        {
            // How many can they afford?
            int maxAffordable = Mathf.FloorToInt(budget / pricePerUnit);

            // How many do they want?
            int desiredQuantity = 1;
            if (itemPreferences.TryGetValue(itemId, out float preference) && preference > 0.5f)
            {
                desiredQuantity = UnityEngine.Random.Range(2, 4);
            }

            if (isImpulsive)
            {
                desiredQuantity *= 2;
            }

            return Mathf.Min(maxAffordable, desiredQuantity);
        }

        public void CompletePurchase(string itemId, int quantity, float totalCost)
        {
            budget -= totalCost;
            shoppingList.Remove(itemId);
            hasShoppedToday = true;

            // Loyalty builds through good experiences
            if (currentMood == Mood.Happy)
            {
                isLoyal = UnityEngine.Random.value > 0.7f;
            }

            Debug.Log($"{customerName} bought {quantity}x {itemId} for ${totalCost:F2}");
        }

        private void GenerateShoppingList()
        {
            // Customers come in looking for 1-3 specific items
            var allItems = Economy.MarketEconomy.Instance.GetAllMarketData().Keys.ToList();
            int itemCount = UnityEngine.Random.Range(1, 4);

            for (int i = 0; i < itemCount && allItems.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, allItems.Count);
                string item = allItems[randomIndex];
                shoppingList.Add(item);
                allItems.RemoveAt(randomIndex);

                // Set random preference
                itemPreferences[item] = UnityEngine.Random.Range(0.3f, 1f);
            }
        }

        private void React(string comment, Mood mood)
        {
            currentMood = mood;
            Debug.Log($"{customerName}: {comment}");
            // This would trigger UI dialogue/reaction animations
        }

        private float GetMoodMultiplier()
        {
            return currentMood switch
            {
                Mood.Happy => 1.2f,
                Mood.Angry => 0.6f,
                Mood.Stressed => 0.8f,
                Mood.Disappointed => 0.7f,
                _ => 1f
            };
        }

        private string GenerateRandomName()
        {
            string[] firstNames = { "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Riley", "Avery", "Quinn", "Sage", "Drew" };
            string[] lastNames = { "Smith", "Johnson", "Chen", "Garcia", "Patel", "Kim", "Martinez", "Lee", "Wilson", "Brown" };

            return $"{firstNames[UnityEngine.Random.Range(0, firstNames.Length)]} {lastNames[UnityEngine.Random.Range(0, lastNames.Length)]}";
        }

        public void LeaveShop(bool satisfied)
        {
            OnLeaving?.Invoke(this);

            if (!satisfied && !hasShoppedToday)
            {
                // Negative word of mouth hurts reputation
                Player.PlayerShop.Instance.GetType()
                    .GetProperty("Reputation")?
                    .SetValue(Player.PlayerShop.Instance,
                        Mathf.Max(0, Player.PlayerShop.Instance.Reputation - 1f));
            }

            Destroy(gameObject, 1f);
        }
    }

    public enum CustomerArchetype
    {
        BargainHunter,      // High price sensitivity, low budget
        PremiumBuyer,       // Low price sensitivity, high budget
        RegularShopper,     // Average across the board
        Impulse Buyer,      // Low price awareness, buys on emotion
        SmartShopper,       // High price awareness, strategic
        Loyalist           // Returns frequently if treated well
    }

    public enum Mood
    {
        Happy,
        Neutral,
        Disappointed,
        Angry,
        Stressed,
        Excited
    }
}
