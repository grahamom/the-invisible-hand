using UnityEngine;
using System.Collections.Generic;
using System;

namespace TheInvisibleHand.NPCs
{
    /// <summary>
    /// Spawns customers based on time of day, reputation, and market conditions
    /// Simulates foot traffic and customer flow
    /// </summary>
    public class CustomerSpawner : MonoBehaviour
    {
        public static CustomerSpawner Instance { get; private set; }

        [Header("Spawn Settings")]
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int maxSimultaneousCustomers = 5;

        [Header("Traffic Patterns")]
        [SerializeField] private AnimationCurve trafficByTimeOfDay;
        [SerializeField] private float baseSpawnRate = 30f; // Seconds between spawns

        private List<Customer> activeCustomers = new List<Customer>();
        private float timeSinceLastSpawn;

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
            Core.GameManager.Instance.OnPhaseChanged += OnPhaseChanged;
            InitializeTrafficCurve();
        }

        private void Update()
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (ShouldSpawnCustomer())
            {
                SpawnCustomer();
                timeSinceLastSpawn = 0f;
            }

            // Clean up customers who have left
            activeCustomers.RemoveAll(c => c == null);
        }

        private bool ShouldSpawnCustomer()
        {
            if (activeCustomers.Count >= maxSimultaneousCustomers)
                return false;

            float currentHour = Core.GameManager.Instance.CurrentTime;
            float trafficMultiplier = trafficByTimeOfDay.Evaluate(currentHour / 24f);

            // Reputation affects foot traffic
            float reputationMultiplier = Player.PlayerShop.Instance.Reputation / 50f; // 0.0 to 2.0

            float adjustedSpawnRate = baseSpawnRate / (trafficMultiplier * reputationMultiplier);

            return timeSinceLastSpawn >= adjustedSpawnRate;
        }

        private void SpawnCustomer()
        {
            if (customerPrefab == null)
            {
                // For now, create a basic GameObject
                var customerObj = new GameObject("Customer");
                var customer = customerObj.AddComponent<Customer>();
                ConfigureCustomer(customer);
                activeCustomers.Add(customer);
            }
            else
            {
                var customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                var customer = customerObj.GetComponent<Customer>();
                if (customer != null)
                {
                    ConfigureCustomer(customer);
                    activeCustomers.Add(customer);
                }
            }
        }

        private void ConfigureCustomer(Customer customer)
        {
            // Assign random archetype based on time and conditions
            customer.archetype = GetRandomArchetype();

            // Configure based on archetype
            switch (customer.archetype)
            {
                case CustomerArchetype.BargainHunter:
                    customer.priceAwareness = UnityEngine.Random.Range(0.7f, 1f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.7f, 1f);
                    customer.dailyBudget = UnityEngine.Random.Range(10f, 30f);
                    break;

                case CustomerArchetype.PremiumBuyer:
                    customer.priceAwareness = UnityEngine.Random.Range(0.3f, 0.6f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.1f, 0.4f);
                    customer.dailyBudget = UnityEngine.Random.Range(50f, 150f);
                    break;

                case CustomerArchetype.ImpulseBuyer:
                    customer.priceAwareness = UnityEngine.Random.Range(0.2f, 0.5f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.3f, 0.6f);
                    customer.dailyBudget = UnityEngine.Random.Range(30f, 80f);
                    customer.isImpulsive = true;
                    break;

                case CustomerArchetype.SmartShopper:
                    customer.priceAwareness = UnityEngine.Random.Range(0.8f, 1f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.5f, 0.8f);
                    customer.dailyBudget = UnityEngine.Random.Range(40f, 100f);
                    break;

                case CustomerArchetype.Loyalist:
                    customer.priceAwareness = UnityEngine.Random.Range(0.4f, 0.7f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.3f, 0.6f);
                    customer.dailyBudget = UnityEngine.Random.Range(30f, 70f);
                    customer.isLoyal = true;
                    break;

                default: // RegularShopper
                    customer.priceAwareness = UnityEngine.Random.Range(0.4f, 0.7f);
                    customer.priceSensitivity = UnityEngine.Random.Range(0.4f, 0.7f);
                    customer.dailyBudget = UnityEngine.Random.Range(20f, 60f);
                    break;
            }

            // Subscribe to events
            customer.OnPurchaseAttempt += OnCustomerPurchaseAttempt;
            customer.OnComplaint += OnCustomerComplaint;

            Debug.Log($"Customer spawned: {customer.customerName} ({customer.archetype})");
        }

        private CustomerArchetype GetRandomArchetype()
        {
            // Distribution can change based on time, day, or events
            float roll = UnityEngine.Random.value;

            if (roll < 0.15f) return CustomerArchetype.BargainHunter;
            if (roll < 0.25f) return CustomerArchetype.PremiumBuyer;
            if (roll < 0.45f) return CustomerArchetype.RegularShopper;
            if (roll < 0.60f) return CustomerArchetype.ImpulseBuyer;
            if (roll < 0.80f) return CustomerArchetype.SmartShopper;
            return CustomerArchetype.Loyalist;
        }

        private void InitializeTrafficCurve()
        {
            if (trafficByTimeOfDay == null || trafficByTimeOfDay.length == 0)
            {
                // Create realistic traffic pattern
                trafficByTimeOfDay = new AnimationCurve(
                    new Keyframe(0.25f, 0.1f),   // 6 AM - Opening, low traffic
                    new Keyframe(0.40f, 1.2f),   // ~9-10 AM - Morning rush
                    new Keyframe(0.50f, 1.5f),   // 12 PM - Lunch peak
                    new Keyframe(0.60f, 0.7f),   // 2-3 PM - Afternoon lull
                    new Keyframe(0.75f, 1.3f),   // 5-6 PM - Evening rush
                    new Keyframe(0.85f, 0.5f)    // 8 PM - Closing time
                );
            }
        }

        private void OnPhaseChanged(Core.GamePhase phase)
        {
            // Could adjust spawn rates or customer types based on phase
            Debug.Log($"Traffic adjusting for {phase} phase");
        }

        private void OnCustomerPurchaseAttempt(Customer customer, string itemId, int quantity)
        {
            // Handle the purchase through PlayerShop
            if (Player.PlayerShop.Instance.SellItem(itemId, quantity, out float total))
            {
                customer.CompletePurchase(itemId, quantity, total);
            }
        }

        private void OnCustomerComplaint(Customer customer, string itemId)
        {
            Debug.Log($"{customer.customerName} complained about {itemId} pricing!");
            // Could trigger UI notifications or reputation events
        }

        public int GetCustomerCount() => activeCustomers.Count;
    }
}
