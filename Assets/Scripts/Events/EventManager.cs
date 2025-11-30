using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TheInvisibleHand.Events
{
    /// <summary>
    /// Manages random events and keeps the game interesting
    /// Events teach economic concepts naturally through gameplay
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        [Header("Event Pool")]
        [SerializeField] private List<MarketEvent> possibleEvents = new List<MarketEvent>();

        [Header("Settings")]
        [SerializeField] private float eventCheckInterval = 60f; // Seconds
        [SerializeField] private int maxEventsPerDay = 3;

        private List<MarketEvent> activeEvents = new List<MarketEvent>();
        private int eventsTriggeredToday = 0;
        private float timeSinceLastCheck = 0f;

        public System.Action<MarketEvent> OnEventTriggered;

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
            CreateSampleEvents();
        }

        private void Update()
        {
            timeSinceLastCheck += Time.deltaTime;

            if (timeSinceLastCheck >= eventCheckInterval)
            {
                CheckForEvents();
                timeSinceLastCheck = 0f;
            }
        }

        private void CheckForEvents()
        {
            if (eventsTriggeredToday >= maxEventsPerDay)
                return;

            int currentDay = Core.GameManager.Instance.CurrentDay;

            // Filter eligible events
            var eligibleEvents = possibleEvents.Where(e =>
                e.minDay <= currentDay &&
                e.maxDay >= currentDay &&
                !activeEvents.Contains(e)
            ).ToList();

            // Roll for event
            foreach (var evt in eligibleEvents)
            {
                if (Random.value < evt.probability)
                {
                    TriggerEvent(evt);
                    break; // Only one event per check
                }
            }
        }

        private void TriggerEvent(MarketEvent evt)
        {
            evt.TriggerEvent();
            activeEvents.Add(evt);
            eventsTriggeredToday++;

            OnEventTriggered?.Invoke(evt);

            // Schedule event expiration
            Invoke(nameof(ExpireOldestEvent), evt.duration * 24f * 3600f / Core.GameManager.Instance.GetType().GetField("gameSpeedMultiplier").GetValue(Core.GameManager.Instance) as float? ?? 1f);
        }

        private void ExpireOldestEvent()
        {
            if (activeEvents.Count > 0)
            {
                var expiredEvent = activeEvents[0];
                activeEvents.RemoveAt(0);
                Debug.Log($"Event expired: {expiredEvent.eventTitle}");
            }
        }

        private void OnNewDay(int day)
        {
            eventsTriggeredToday = 0;
        }

        /// <summary>
        /// Create some sample events for demonstration
        /// In a real game, these would be ScriptableObject assets
        /// </summary>
        private void CreateSampleEvents()
        {
            // Heat Wave - increases demand for beverages
            var heatWave = ScriptableObject.CreateInstance<MarketEvent>();
            heatWave.eventId = "heat_wave";
            heatWave.eventTitle = "Heat Wave Hits City";
            heatWave.eventDescription = "Temperatures soar! Citizens desperate for cold drinks.";
            heatWave.probability = 0.05f;
            heatWave.duration = 2f;
            heatWave.impacts = new MarketImpact[]
            {
                new MarketImpact
                {
                    affectedItem = "Coffee",
                    impactType = ImpactType.DemandIncrease,
                    magnitude = 0.8f,
                    explanation = "Hot weather increases demand for cold beverages"
                },
                new MarketImpact
                {
                    affectedItem = "Milk",
                    impactType = ImpactType.DemandIncrease,
                    magnitude = 0.5f,
                    explanation = "People buying more milk for cold drinks"
                }
            };
            possibleEvents.Add(heatWave);

            // Harvest Festival
            var harvest = ScriptableObject.CreateInstance<MarketEvent>();
            harvest.eventId = "harvest_festival";
            harvest.eventTitle = "Harvest Festival";
            harvest.eventDescription = "Local farms bring in bumper crops! Fresh produce floods the market.";
            harvest.probability = 0.04f;
            harvest.duration = 3f;
            harvest.impacts = new MarketImpact[]
            {
                new MarketImpact
                {
                    affectedItem = "Apples",
                    impactType = ImpactType.SupplySurplus,
                    magnitude = 1.5f,
                    explanation = "Abundant harvest creates supply surplus, lowering prices"
                }
            };
            possibleEvents.Add(harvest);

            // Artisan Fair
            var artisanFair = ScriptableObject.CreateInstance<MarketEvent>();
            artisanFair.eventId = "artisan_fair";
            artisanFair.eventTitle = "Artisan Fair This Weekend";
            artisanFair.eventDescription = "Tourists flock to the city for the famous artisan fair!";
            artisanFair.probability = 0.03f;
            artisanFair.duration = 2f;
            artisanFair.impacts = new MarketImpact[]
            {
                new MarketImpact
                {
                    affectedItem = "Cheese",
                    impactType = ImpactType.DemandIncrease,
                    magnitude = 1.0f,
                    explanation = "Tourists seek local specialties"
                },
                new MarketImpact
                {
                    affectedItem = "Wine",
                    impactType = ImpactType.DemandIncrease,
                    magnitude = 1.2f,
                    explanation = "Premium goods in high demand during events"
                }
            };
            possibleEvents.Add(artisanFair);

            // Supply Chain Disruption
            var disruption = ScriptableObject.CreateInstance<MarketEvent>();
            disruption.eventId = "supply_disruption";
            disruption.eventTitle = "Supply Chain Issues";
            disruption.eventDescription = "Delivery trucks delayed! Wholesale shortages reported.";
            disruption.probability = 0.06f;
            disruption.duration = 1f;
            disruption.impacts = new MarketImpact[]
            {
                new MarketImpact
                {
                    affectedItem = "Bread",
                    impactType = ImpactType.SupplyShock,
                    magnitude = 0.6f,
                    explanation = "Supply disruptions cause scarcity and price increases"
                },
                new MarketImpact
                {
                    affectedItem = "Milk",
                    impactType = ImpactType.SupplyShock,
                    magnitude = 0.5f,
                    explanation = "Perishables especially affected by delivery delays"
                }
            };
            possibleEvents.Add(disruption);

            Debug.Log($"Loaded {possibleEvents.Count} market events");
        }

        public void TriggerSpecificEvent(string eventId)
        {
            var evt = possibleEvents.FirstOrDefault(e => e.eventId == eventId);
            if (evt != null)
            {
                TriggerEvent(evt);
            }
        }
    }
}
