using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TheInvisibleHand.Core
{
    /// <summary>
    /// Manages player progression, unlockables, and achievements
    /// Keeps players engaged with goals and rewards
    /// </summary>
    public class ProgressionManager : MonoBehaviour
    {
        public static ProgressionManager Instance { get; private set; }

        [Header("Unlockables")]
        [SerializeField] private List<Unlockable> unlockables = new List<Unlockable>();

        [Header("Achievements")]
        [SerializeField] private List<Achievement> achievements = new List<Achievement>();

        private HashSet<string> unlockedIds = new HashSet<string>();
        private HashSet<string> achievedIds = new HashSet<string>();

        public event Action<Unlockable> OnUnlocked;
        public event Action<Achievement> OnAchievement;

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
            InitializeProgressionSystem();
            SubscribeToEvents();
        }

        private void InitializeProgressionSystem()
        {
            // Create unlockables
            unlockables = new List<Unlockable>
            {
                new Unlockable
                {
                    id = "premium_items",
                    displayName = "Premium Goods",
                    description = "Unlock Wine and Cheese for sale",
                    unlockCondition = UnlockCondition.ReachLevel,
                    requiredValue = 3
                },
                new Unlockable
                {
                    id = "market_stall_upgrade",
                    displayName = "Larger Stall",
                    description = "+5 display slots",
                    unlockCondition = UnlockCondition.ReachLevel,
                    requiredValue = 5
                },
                new Unlockable
                {
                    id = "price_insights",
                    displayName = "Price Predictions",
                    description = "See forecasted price trends",
                    unlockCondition = UnlockCondition.MakeSales,
                    requiredValue = 50
                },
                new Unlockable
                {
                    id = "bulk_pricing",
                    displayName = "Bulk Discounts",
                    description = "Offer quantity discounts to customers",
                    unlockCondition = UnlockCondition.ReachReputation,
                    requiredValue = 75
                },
                new Unlockable
                {
                    id = "competitor_intel",
                    displayName = "Market Intelligence",
                    description = "See what competitors are charging",
                    unlockCondition = UnlockCondition.EarnMoney,
                    requiredValue = 500
                },
                new Unlockable
                {
                    id = "seasonal_items",
                    displayName = "Seasonal Trading",
                    description = "Access to seasonal exclusive items",
                    unlockCondition = UnlockCondition.SurviveDays,
                    requiredValue = 30
                }
            };

            // Create achievements
            achievements = new List<Achievement>
            {
                new Achievement
                {
                    id = "first_sale",
                    title = "First Customer",
                    description = "Make your first sale",
                    emoji = "🎉"
                },
                new Achievement
                {
                    id = "perfect_price",
                    title = "Perfect Price",
                    description = "Price an item within 5% of optimal market price",
                    emoji = "🎯"
                },
                new Achievement
                {
                    id = "bargain_hunter",
                    title = "Bargain Hunter",
                    description = "Buy 10 items during a surplus",
                    emoji = "💰"
                },
                new Achievement
                {
                    id = "market_master",
                    title = "Market Master",
                    description = "Successfully profit from a shortage",
                    emoji = "📈"
                },
                new Achievement
                {
                    id = "customer_favorite",
                    title = "Customer Favorite",
                    description = "Reach 90+ reputation",
                    emoji = "⭐"
                },
                new Achievement
                {
                    id = "merchant_tycoon",
                    title = "Merchant Tycoon",
                    description = "Earn $1000 total profit",
                    emoji = "👑"
                },
                new Achievement
                {
                    id = "economist",
                    title = "Economist",
                    description = "Witness all market conditions",
                    emoji = "🎓"
                },
                new Achievement
                {
                    id = "crisis_manager",
                    title = "Crisis Manager",
                    description = "Survive and profit during 3 market events",
                    emoji = "🛡️"
                }
            };

            Debug.Log($"Initialized {unlockables.Count} unlockables and {achievements.Count} achievements");
        }

        private void SubscribeToEvents()
        {
            // Level ups
            Player.PlayerShop.Instance.OnLevelUp += CheckLevelUnlocks;

            // Reputation changes
            Player.PlayerShop.Instance.OnReputationChanged += CheckReputationUnlocks;

            // Money changes
            Player.PlayerInventory.Instance.OnMoneyChanged += CheckMoneyUnlocks;

            // New day
            GameManager.Instance.OnNewDay += CheckDayUnlocks;
        }

        private void CheckLevelUnlocks(int level)
        {
            CheckUnlocks(UnlockCondition.ReachLevel, level);
        }

        private void CheckReputationUnlocks(float reputation)
        {
            CheckUnlocks(UnlockCondition.ReachReputation, reputation);

            // Achievement check
            if (reputation >= 90f)
            {
                TriggerAchievement("customer_favorite");
            }
        }

        private void CheckMoneyUnlocks(float money)
        {
            CheckUnlocks(UnlockCondition.EarnMoney, money);

            // Achievement checks
            if (money >= 1000f)
            {
                TriggerAchievement("merchant_tycoon");
            }
        }

        private void CheckDayUnlocks(int day)
        {
            CheckUnlocks(UnlockCondition.SurviveDays, day);
        }

        private void CheckUnlocks(UnlockCondition condition, float value)
        {
            var eligibleUnlocks = unlockables.Where(u =>
                !unlockedIds.Contains(u.id) &&
                u.unlockCondition == condition &&
                value >= u.requiredValue
            ).ToList();

            foreach (var unlock in eligibleUnlocks)
            {
                UnlockFeature(unlock);
            }
        }

        private void UnlockFeature(Unlockable unlock)
        {
            unlockedIds.Add(unlock.id);
            Debug.Log($"🔓 UNLOCKED: {unlock.displayName} - {unlock.description}");

            OnUnlocked?.Invoke(unlock);

            // Apply the unlock
            ApplyUnlock(unlock);
        }

        private void ApplyUnlock(Unlockable unlock)
        {
            switch (unlock.id)
            {
                case "market_stall_upgrade":
                    Player.PlayerShop.Instance.UpgradeDisplaySlots(5);
                    break;

                case "premium_items":
                    // Would add premium items to available inventory
                    Debug.Log("Premium items now available for trading!");
                    break;

                // ... other unlocks
            }
        }

        public void TriggerAchievement(string achievementId)
        {
            if (achievedIds.Contains(achievementId))
                return;

            var achievement = achievements.FirstOrDefault(a => a.id == achievementId);
            if (achievement == null)
                return;

            achievedIds.Add(achievementId);
            Debug.Log($"{achievement.emoji} ACHIEVEMENT: {achievement.title}");
            Debug.Log(achievement.description);

            OnAchievement?.Invoke(achievement);
        }

        public bool IsUnlocked(string unlockId) => unlockedIds.Contains(unlockId);
        public bool IsAchieved(string achievementId) => achievedIds.Contains(achievementId);

        public List<Unlockable> GetAllUnlockables() => new List<Unlockable>(unlockables);
        public List<Achievement> GetAllAchievements() => new List<Achievement>(achievements);
        public List<Achievement> GetAchievedAchievements() =>
            achievements.Where(a => achievedIds.Contains(a.id)).ToList();

        public float GetCompletionPercentage()
        {
            int totalGoals = unlockables.Count + achievements.Count;
            int completed = unlockedIds.Count + achievedIds.Count;
            return (float)completed / totalGoals * 100f;
        }
    }

    [Serializable]
    public class Unlockable
    {
        public string id;
        public string displayName;
        public string description;
        public UnlockCondition unlockCondition;
        public float requiredValue;
        public Sprite icon;
    }

    [Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public string emoji;
        public Sprite icon;
    }

    public enum UnlockCondition
    {
        ReachLevel,
        ReachReputation,
        EarnMoney,
        MakeSales,
        SurviveDays,
        CompleteEvent
    }
}
