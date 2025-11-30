using UnityEngine;
using System.Collections.Generic;
using System;

namespace TheInvisibleHand.Tutorial
{
    /// <summary>
    /// Manages tutorial flow - teaches economic concepts through guided gameplay
    /// Not preachy, just helpful hints and goals
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [Header("Tutorial Settings")]
        [SerializeField] private bool enableTutorial = true;
        [SerializeField] private List<TutorialStep> tutorialSteps;

        private int currentStepIndex = 0;
        private bool tutorialCompleted = false;

        public event Action<TutorialStep> OnStepStarted;
        public event Action<TutorialStep> OnStepCompleted;
        public event Action OnTutorialCompleted;

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
            if (enableTutorial)
            {
                InitializeTutorial();
                StartNextStep();
            }
        }

        private void InitializeTutorial()
        {
            tutorialSteps = new List<TutorialStep>
            {
                new TutorialStep
                {
                    stepId = "welcome",
                    title = "Welcome to The Invisible Hand",
                    description = "You've just inherited a small market stall. Time to make your fortune! Let's start by checking what you have.",
                    goalType = GoalType.OpenInventory,
                    hint = "Tap the Inventory button to see your starting goods"
                },
                new TutorialStep
                {
                    stepId = "check_market",
                    title = "Know Your Market",
                    description = "Smart merchants watch market prices. Let's see what people are buying and selling.",
                    goalType = GoalType.OpenMarketDashboard,
                    hint = "Open the Market Dashboard to see current prices"
                },
                new TutorialStep
                {
                    stepId = "first_purchase",
                    title = "Stock Your Shelves",
                    description = "Buy low, sell high - that's the game. Purchase some goods from the wholesale market.",
                    goalType = GoalType.BuyItem,
                    requiredQuantity = 1,
                    hint = "Look for items with good profit margins (wholesale vs. market price)"
                },
                new TutorialStep
                {
                    stepId = "set_price",
                    title = "Price It Right",
                    description = "Too expensive? Customers walk away. Too cheap? You lose profit. Find the sweet spot.",
                    goalType = GoalType.SetPrice,
                    hint = "Try pricing slightly below market price for your first sale"
                },
                new TutorialStep
                {
                    stepId = "first_sale",
                    title = "Make That Money",
                    description = "Wait for customers to browse your shop. If the price is right, they'll buy!",
                    goalType = GoalType.MakeSale,
                    requiredQuantity = 1,
                    hint = "Customer behavior depends on their budget and price awareness"
                },
                new TutorialStep
                {
                    stepId = "understand_supply_demand",
                    title = "The Invisible Hand",
                    description = "Notice how prices change? That's supply and demand at work. Scarcity raises prices, surplus lowers them.",
                    goalType = GoalType.WitnessMarketChange,
                    hint = "Watch the market dashboard for opportunities"
                },
                new TutorialStep
                {
                    stepId = "profit",
                    title = "Economics 101",
                    description = "Make $50 in profit. Remember: profit = revenue - costs. Track your margins!",
                    goalType = GoalType.ReachMoney,
                    requiredQuantity = 150, // Started with 100, need to reach 150
                    hint = "Buy when prices are low, sell when demand is high"
                }
            };
        }

        private void StartNextStep()
        {
            if (currentStepIndex >= tutorialSteps.Count)
            {
                CompleteTutorial();
                return;
            }

            var step = tutorialSteps[currentStepIndex];
            Debug.Log($"Tutorial Step {currentStepIndex + 1}: {step.title}");
            Debug.Log(step.description);

            OnStepStarted?.Invoke(step);

            // Subscribe to relevant events based on goal type
            SubscribeToStepEvents(step);
        }

        private void SubscribeToStepEvents(TutorialStep step)
        {
            // This would subscribe to the appropriate game events
            // For brevity, showing the concept:
            switch (step.goalType)
            {
                case GoalType.MakeSale:
                    Player.PlayerShop.Instance.OnPriceSet += CheckSaleGoal;
                    break;
                case GoalType.ReachMoney:
                    Player.PlayerInventory.Instance.OnMoneyChanged += CheckMoneyGoal;
                    break;
                // ... other goal types
            }
        }

        private void CheckSaleGoal(string itemId, float price)
        {
            // Logic to check if sale goal is met
            CompleteCurrentStep();
        }

        private void CheckMoneyGoal(float currentMoney)
        {
            var step = tutorialSteps[currentStepIndex];
            if (step.goalType == GoalType.ReachMoney && currentMoney >= step.requiredQuantity)
            {
                CompleteCurrentStep();
            }
        }

        public void CompleteCurrentStep()
        {
            if (currentStepIndex >= tutorialSteps.Count) return;

            var completedStep = tutorialSteps[currentStepIndex];
            Debug.Log($"Tutorial step completed: {completedStep.title}");

            OnStepCompleted?.Invoke(completedStep);

            currentStepIndex++;
            StartNextStep();
        }

        private void CompleteTutorial()
        {
            tutorialCompleted = true;
            Debug.Log("Tutorial completed! You're ready to build your merchant empire.");
            OnTutorialCompleted?.Invoke();
        }

        public void SkipTutorial()
        {
            tutorialCompleted = true;
            currentStepIndex = tutorialSteps.Count;
            Debug.Log("Tutorial skipped");
        }

        public TutorialStep GetCurrentStep()
        {
            if (currentStepIndex < tutorialSteps.Count)
                return tutorialSteps[currentStepIndex];
            return null;
        }

        public bool IsTutorialActive() => enableTutorial && !tutorialCompleted;
    }

    [Serializable]
    public class TutorialStep
    {
        public string stepId;
        public string title;
        [TextArea(2, 4)]
        public string description;
        public string hint;
        public GoalType goalType;
        public int requiredQuantity = 1;
        public string targetItemId;
    }

    public enum GoalType
    {
        OpenInventory,
        OpenMarketDashboard,
        BuyItem,
        SetPrice,
        MakeSale,
        ReachMoney,
        ReachReputation,
        WitnessMarketChange,
        SurviveEvent
    }
}
