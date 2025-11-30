using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TheInvisibleHand.UI
{
    /// <summary>
    /// Main UI controller - coordinates all UI panels and displays
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Main Panels")]
        public GameObject shopPanel;
        public GameObject marketDashboardPanel;
        public GameObject inventoryPanel;
        public GameObject settingsPanel;

        [Header("HUD Elements")]
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI reputationText;
        public Image reputationBar;

        [Header("Notifications")]
        public GameObject notificationPrefab;
        public Transform notificationContainer;
        public float notificationDuration = 3f;

        [Header("Customer Display")]
        public Transform customerQueueContainer;
        public GameObject customerCardPrefab;

        private Dictionary<string, GameObject> activeCustomerCards = new Dictionary<string, GameObject>();

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
            InitializeUI();
            SubscribeToEvents();
        }

        private void InitializeUI()
        {
            // Start with shop panel open
            ShowPanel("shop");
            UpdateHUD();
        }

        private void SubscribeToEvents()
        {
            // Money updates
            if (Player.PlayerInventory.Instance != null)
            {
                Player.PlayerInventory.Instance.OnMoneyChanged += UpdateMoney;
            }

            // Time updates
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnTimeProgressed += UpdateTime;
                Core.GameManager.Instance.OnNewDay += UpdateDay;
                Core.GameManager.Instance.OnPhaseChanged += OnPhaseChanged;
            }

            // Reputation updates
            if (Player.PlayerShop.Instance != null)
            {
                Player.PlayerShop.Instance.OnReputationChanged += UpdateReputation;
                Player.PlayerShop.Instance.OnLevelUp += OnLevelUp;
            }

            // Market events
            if (Events.EventManager.Instance != null)
            {
                Events.EventManager.Instance.OnEventTriggered += ShowEventNotification;
            }

            // Economy changes
            if (Economy.MarketEconomy.Instance != null)
            {
                Economy.MarketEconomy.Instance.OnPriceChanged += OnPriceChanged;
                Economy.MarketEconomy.Instance.OnMarketConditionChanged += OnMarketConditionChanged;
            }
        }

        private void Update()
        {
            // Update HUD every frame (could optimize to only when values change)
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (moneyText != null && Player.PlayerInventory.Instance != null)
                moneyText.text = $"${Player.PlayerInventory.Instance.Money:F2}";

            if (dayText != null && Core.GameManager.Instance != null)
                dayText.text = $"Day {Core.GameManager.Instance.CurrentDay}";

            if (timeText != null && Core.GameManager.Instance != null)
            {
                float time = Core.GameManager.Instance.CurrentTime;
                int hours = Mathf.FloorToInt(time);
                int minutes = Mathf.FloorToInt((time - hours) * 60f);
                timeText.text = $"{hours:00}:{minutes:00}";
            }

            if (reputationText != null && Player.PlayerShop.Instance != null)
            {
                float rep = Player.PlayerShop.Instance.Reputation;
                reputationText.text = $"Rep: {rep:F0}";

                if (reputationBar != null)
                    reputationBar.fillAmount = rep / 100f;
            }
        }

        public void ShowPanel(string panelName)
        {
            // Hide all panels
            if (shopPanel != null) shopPanel.SetActive(false);
            if (marketDashboardPanel != null) marketDashboardPanel.SetActive(false);
            if (inventoryPanel != null) inventoryPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);

            // Show requested panel
            switch (panelName.ToLower())
            {
                case "shop":
                    if (shopPanel != null) shopPanel.SetActive(true);
                    break;
                case "market":
                    if (marketDashboardPanel != null) marketDashboardPanel.SetActive(true);
                    break;
                case "inventory":
                    if (inventoryPanel != null) inventoryPanel.SetActive(true);
                    break;
                case "settings":
                    if (settingsPanel != null) settingsPanel.SetActive(true);
                    break;
            }
        }

        public void ShowNotification(string message, NotificationType type = NotificationType.Info)
        {
            Debug.Log($"[{type}] {message}");

            // In a real implementation, this would create a visual notification
            // For now, just log it
            // TODO: Create animated notification UI
        }

        private void ShowEventNotification(Events.MarketEvent evt)
        {
            ShowNotification($"📰 {evt.eventTitle}: {evt.eventDescription}", NotificationType.Event);
        }

        private void UpdateMoney(float newAmount)
        {
            if (moneyText != null)
                moneyText.text = $"${newAmount:F2}";
        }

        private void UpdateTime(float time)
        {
            // Updated in UpdateHUD
        }

        private void UpdateDay(int day)
        {
            if (dayText != null)
                dayText.text = $"Day {day}";

            ShowNotification($"☀️ Day {day} begins!", NotificationType.Info);
        }

        private void UpdateReputation(float reputation)
        {
            if (reputationText != null)
                reputationText.text = $"Rep: {reputation:F0}";

            if (reputationBar != null)
                reputationBar.fillAmount = reputation / 100f;
        }

        private void OnLevelUp(int newLevel)
        {
            ShowNotification($"🎉 Level Up! You're now level {newLevel}!", NotificationType.Success);
        }

        private void OnPhaseChanged(Core.GamePhase phase)
        {
            string phaseMessage = phase switch
            {
                Core.GamePhase.Opening => "🌅 Shop opening - time to prepare!",
                Core.GamePhase.MorningRush => "☕ Morning rush begins!",
                Core.GamePhase.Lunch => "🍽️ Lunch hour - peak traffic!",
                Core.GamePhase.Afternoon => "📦 Afternoon lull - good time to restock",
                Core.GamePhase.EveningRush => "🌆 Evening rush incoming!",
                Core.GamePhase.Closing => "🌙 Closing time approaching",
                Core.GamePhase.Night => "💤 Shop closed for the night",
                _ => ""
            };

            if (!string.IsNullOrEmpty(phaseMessage))
                ShowNotification(phaseMessage, NotificationType.Info);
        }

        private void OnPriceChanged(string itemId, float newPrice)
        {
            // Optionally notify about significant price changes
        }

        private void OnMarketConditionChanged(string itemId, Economy.MarketCondition condition)
        {
            string emoji = condition switch
            {
                Economy.MarketCondition.Shortage => "📈",
                Economy.MarketCondition.Surplus => "📉",
                Economy.MarketCondition.Rising => "⬆️",
                Economy.MarketCondition.Falling => "⬇️",
                _ => "➡️"
            };

            ShowNotification($"{emoji} {itemId} market is now {condition}", NotificationType.Market);
        }

        public void TogglePause()
        {
            Core.GameManager.Instance.TogglePause();
        }
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error,
        Event,
        Market
    }
}
