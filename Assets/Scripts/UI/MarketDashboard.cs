using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace TheInvisibleHand.UI
{
    /// <summary>
    /// Market overview dashboard - shows supply/demand charts, price trends, predictions
    /// This is where players learn to "read the market"
    /// </summary>
    public class MarketDashboard : MonoBehaviour
    {
        [Header("Market Items Display")]
        public Transform itemListContainer;
        public GameObject marketItemCardPrefab;

        [Header("Chart Display")]
        public Image priceChartImage;
        public TextMeshProUGUI chartTitle;

        [Header("Market Summary")]
        public TextMeshProUGUI marketSummaryText;

        private Dictionary<string, MarketItemCard> itemCards = new Dictionary<string, MarketItemCard>();
        private string selectedItemId;

        private void Start()
        {
            RefreshMarketDisplay();
            InvokeRepeating(nameof(RefreshMarketDisplay), 5f, 5f);
        }

        public void RefreshMarketDisplay()
        {
            var marketData = Economy.MarketEconomy.Instance.GetAllMarketData();

            foreach (var kvp in marketData)
            {
                UpdateOrCreateItemCard(kvp.Key, kvp.Value);
            }

            UpdateMarketSummary(marketData);
        }

        private void UpdateOrCreateItemCard(string itemId, Economy.MarketData data)
        {
            if (!itemCards.ContainsKey(itemId))
            {
                // Create new card (in real implementation, would instantiate prefab)
                // For now, just track the data
                itemCards[itemId] = new MarketItemCard { itemId = itemId };
            }

            var card = itemCards[itemId];
            card.UpdateData(data);
        }

        private void UpdateMarketSummary(Dictionary<string, Economy.MarketData> marketData)
        {
            if (marketSummaryText == null) return;

            int shortages = marketData.Count(d => d.Value.Condition == Economy.MarketCondition.Shortage);
            int surpluses = marketData.Count(d => d.Value.Condition == Economy.MarketCondition.Surplus);
            int stable = marketData.Count(d => d.Value.Condition == Economy.MarketCondition.Stable);

            string summary = $"Market Overview\n\n";
            summary += $"📊 Total Items: {marketData.Count}\n";
            summary += $"⚠️ Shortages: {shortages}\n";
            summary += $"📦 Surpluses: {surpluses}\n";
            summary += $"✅ Stable: {stable}\n\n";

            // Find best opportunities
            var bestBuy = marketData
                .OrderBy(d => d.Value.CurrentPrice / d.Value.BasePrice)
                .FirstOrDefault();

            var bestSell = marketData
                .OrderByDescending(d => d.Value.CurrentPrice / d.Value.BasePrice)
                .FirstOrDefault();

            if (bestBuy.Value != null)
            {
                float discount = (1f - (bestBuy.Value.CurrentPrice / bestBuy.Value.BasePrice)) * 100f;
                summary += $"💰 Best Buy: {bestBuy.Key} ({discount:F0}% below base)\n";
            }

            if (bestSell.Value != null)
            {
                float premium = ((bestSell.Value.CurrentPrice / bestSell.Value.BasePrice) - 1f) * 100f;
                summary += $"💎 Best Sell: {bestSell.Key} ({premium:F0}% above base)\n";
            }

            marketSummaryText.text = summary;
        }

        public void SelectItem(string itemId)
        {
            selectedItemId = itemId;
            DisplayItemChart(itemId);
        }

        private void DisplayItemChart(string itemId)
        {
            var data = Economy.MarketEconomy.Instance.GetMarketData(itemId);
            if (data == null) return;

            if (chartTitle != null)
                chartTitle.text = $"{itemId} Market Analysis";

            // In a real implementation, would draw price/supply/demand charts
            // For now, just show the data
            Debug.Log($"Chart for {itemId}:");
            Debug.Log($"  Current Price: ${data.CurrentPrice:F2} (Base: ${data.BasePrice:F2})");
            Debug.Log($"  Supply: {data.Supply:F0} | Demand: {data.CurrentDemand:F0}");
            Debug.Log($"  Condition: {data.Condition}");
        }
    }

    // Helper class for market item display
    public class MarketItemCard
    {
        public string itemId;
        public Economy.MarketData currentData;

        public void UpdateData(Economy.MarketData data)
        {
            currentData = data;
        }

        public string GetDisplayText()
        {
            if (currentData == null) return "";

            string conditionEmoji = currentData.Condition switch
            {
                Economy.MarketCondition.Shortage => "🔴",
                Economy.MarketCondition.Surplus => "🟢",
                Economy.MarketCondition.Rising => "📈",
                Economy.MarketCondition.Falling => "📉",
                _ => "⚪"
            };

            float priceChange = ((currentData.CurrentPrice / currentData.BasePrice) - 1f) * 100f;
            string priceChangeStr = priceChange >= 0 ? $"+{priceChange:F0}%" : $"{priceChange:F0}%";

            return $"{conditionEmoji} {itemId}\n${currentData.CurrentPrice:F2} ({priceChangeStr})";
        }
    }
}
