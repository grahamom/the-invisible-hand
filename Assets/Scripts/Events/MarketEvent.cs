using UnityEngine;
using System;

namespace TheInvisibleHand.Events
{
    /// <summary>
    /// Random market events that teach economic concepts through gameplay
    /// Examples: supply shocks, demand surges, competitor actions, weather, news
    /// </summary>
    [CreateAssetMenu(fileName = "NewMarketEvent", menuName = "The Invisible Hand/Market Event")]
    public class MarketEvent : ScriptableObject
    {
        [Header("Event Info")]
        public string eventId;
        public string eventTitle;
        [TextArea(3, 5)]
        public string eventDescription;
        public Sprite eventIcon;

        [Header("Trigger Conditions")]
        public int minDay = 1;
        public int maxDay = 999;
        [Range(0f, 1f)]
        public float probability = 0.1f;
        public EventTriggerType triggerType;

        [Header("Economic Impact")]
        public MarketImpact[] impacts;
        public float duration = 1f; // Days the effect lasts

        [Header("Flavor")]
        public string[] headlines; // News-style headlines
        public Color eventColor = Color.white;

        public void TriggerEvent()
        {
            Debug.Log($"EVENT: {eventTitle}");
            Debug.Log(eventDescription);

            foreach (var impact in impacts)
            {
                ApplyImpact(impact);
            }
        }

        private void ApplyImpact(MarketImpact impact)
        {
            var marketData = Economy.MarketEconomy.Instance.GetMarketData(impact.affectedItem);
            if (marketData == null) return;

            switch (impact.impactType)
            {
                case ImpactType.DemandIncrease:
                    marketData.CurrentDemand *= (1f + impact.magnitude);
                    Debug.Log($"Demand for {impact.affectedItem} increased by {impact.magnitude * 100}%");
                    break;

                case ImpactType.DemandDecrease:
                    marketData.CurrentDemand *= (1f - impact.magnitude);
                    Debug.Log($"Demand for {impact.affectedItem} decreased by {impact.magnitude * 100}%");
                    break;

                case ImpactType.SupplyShock:
                    marketData.Supply *= (1f - impact.magnitude);
                    Debug.Log($"Supply of {impact.affectedItem} reduced by {impact.magnitude * 100}%");
                    break;

                case ImpactType.SupplySurplus:
                    marketData.Supply *= (1f + impact.magnitude);
                    Debug.Log($"Supply of {impact.affectedItem} increased by {impact.magnitude * 100}%");
                    break;

                case ImpactType.PriceFloor:
                    // Sets minimum price (government intervention)
                    float minPrice = impact.magnitude;
                    if (marketData.CurrentPrice < minPrice)
                        Debug.Log($"Price floor set for {impact.affectedItem}: ${minPrice}");
                    break;

                case ImpactType.PriceCeiling:
                    // Sets maximum price (government intervention)
                    float maxPrice = impact.magnitude;
                    if (marketData.CurrentPrice > maxPrice)
                        Debug.Log($"Price ceiling set for {impact.affectedItem}: ${maxPrice}");
                    break;
            }
        }
    }

    [Serializable]
    public class MarketImpact
    {
        public string affectedItem;
        public ImpactType impactType;
        public float magnitude; // Percentage or absolute value depending on type
        public string explanation; // Educational explanation
    }

    public enum EventTriggerType
    {
        Random,
        TimeOfDay,
        DayOfWeek,
        PlayerAction,
        MarketCondition
    }

    public enum ImpactType
    {
        DemandIncrease,
        DemandDecrease,
        SupplyShock,
        SupplySurplus,
        PriceFloor,
        PriceCeiling,
        CompetitorAction
    }
}
